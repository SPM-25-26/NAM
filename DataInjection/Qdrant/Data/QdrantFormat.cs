using Microsoft.Extensions.VectorData;

namespace DataInjection.Qdrant.Data
{
    public class QdrantFormat
    {

        [VectorStoreKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        [VectorStoreVector(Dimensions: 3072)]
        public ReadOnlyMemory<float> Vector { get; set; }

        [VectorStoreData]
        public int number { get; set; }
    }
}
