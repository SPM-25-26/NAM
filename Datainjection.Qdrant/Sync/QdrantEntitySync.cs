using DataInjection.Core.Interfaces;
using DataInjection.Qdrant.Data;
using Microsoft.Extensions.VectorData;

namespace Datainjection.Qdrant.Sync
{
    public class QdrantEntitySync
        (
        Serilog.ILogger logger,
        IConfiguration configuration,
        VectorStoreCollection<Guid, POIEntity> store,
        IEnumerable<IEntityCollector<POIEntity>> entityCollectors
        )
    {

        public async Task ExecuteSyncAsync()
        {
            await store.EnsureCollectionExistsAsync();

            var municipalities = configuration.GetSection("Municipalities").Get<string[]>() ?? [];
            if (municipalities.Length == 0)
            {
                logger.Warning("Empty list of municipalities");
                return;
            }

            foreach (var municipality in municipalities)
            {
                foreach (var collector in entityCollectors)
                {
                    logger.Information("Starting qdrant sync for {Collector}", collector);

                    try
                    {
                        var entities = await collector.GetEntities(municipality);
                        await store.UpsertAsync(entities);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Error fetching data for municipality {Municipality}: {ErrorMessage}", municipality, ex.Message);
                    }
                    logger.Information("Successfully synced qdrant data for {Collector}", collector);

                }
                logger.Information("Successfully injected data of municipality {Municipality}", municipality);
            }
        }
    }
}
