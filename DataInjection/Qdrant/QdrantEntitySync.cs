using DataInjection.Interfaces;
using DataInjection.Qdrant.Embedders;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace DataInjection.Qdrant
{
    internal class QdrantEntitySync : ISyncService
    {
        private readonly QdrantClient client;
        private IEmbedder embedder;

        public QdrantEntitySync(ulong OutputDimensionality)
        {
            this.client = new QdrantClient("localhost", 55080);
            client.CreateCollectionAsync("test_collection", new VectorParams { Size = OutputDimensionality, Distance = Distance.Cosine }).Wait();
            embedder = GeminiEmbedder.Instance;
        }

        public async Task ExecuteSyncAsync<TEntity>(IEntityCollector<TEntity> entityCollector) where TEntity : class
        {
            await entityCollector.GetEntities("Matelica");
        }
    }
}
