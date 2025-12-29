using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using Microsoft.Extensions.VectorData;
using System.Collections.Concurrent;

namespace DataInjection.Qdrant
{
    public class QdrantEntitySync(Serilog.ILogger logger, VectorStoreCollection<Guid, QdrantFormat> store, string collectionName)
    {

        public async Task ExecuteSyncAsync(IEntityCollector<QdrantFormat> entityCollector)
        {
            await store.EnsureCollectionExistsAsync();

            var municipalities = new List<string> { "Matelica" };//configuration.GetSection("Municipalities").Get<string[]>() ?? [];
            var allEntities = new ConcurrentBag<QdrantFormat>();

            await Parallel.ForEachAsync(municipalities, async (municipality, ct) =>
            {
                try
                {
                    var entities = await entityCollector.GetEntities(municipality);
                    if (entities != null)
                    {
                        foreach (var e in entities) allEntities.Add(e);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Error fetching data for municipality {Municipality}: {ErrorMessage}", municipality, ex.Message);
                }
            });

            if (allEntities.IsEmpty)
            {
                logger.Warning("No entities were fetched. Aborting synchronization.");
                return;
            }

            await store.UpsertAsync(allEntities.ToList());

        }
    }
}
