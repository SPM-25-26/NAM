using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using Microsoft.Extensions.VectorData;
using System.Collections.Concurrent;

namespace DataInjection.Qdrant
{
    public class QdrantEntitySync(Serilog.ILogger logger, IConfiguration configuration, VectorStoreCollection<Guid, POIEntity> store)
    {

        public async Task ExecuteSyncAsync(IEntityCollector<POIEntity> entityCollector)
        {
            await store.EnsureCollectionExistsAsync();

            var municipalities = configuration.GetSection("Municipalities").Get<string[]>() ?? [];
            var allEntities = new ConcurrentBag<POIEntity>();

            foreach (var municipality in municipalities)
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
            }

            if (allEntities.IsEmpty)
            {
                logger.Warning("[Qdrant] No entities were fetched. Aborting synchronization.");
                return;
            }

            await store.UpsertAsync(allEntities);

        }
    }
}
