using Microsoft.Extensions.VectorData;

namespace DataInjection.Qdrant.Data
{
    public class POIEntity
    {

        [VectorStoreKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        [VectorStoreVector(Dimensions: 3072)]
        public ReadOnlyMemory<float> Vector { get; set; }

        [VectorStoreData]
        public int chunkPart { get; set; }

        [VectorStoreData]
        public string apiEndpoint { get; set; }

        [VectorStoreData]
        public string EntityId { get; set; }

        [VectorStoreData]
        public string city { get; set; }

        [VectorStoreData]
        public double lat { get; set; }

        [VectorStoreData]
        public double lon { get; set; }
    }
}
