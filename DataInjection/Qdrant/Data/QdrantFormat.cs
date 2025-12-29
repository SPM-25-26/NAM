namespace DataInjection.Qdrant.Data
{
    public record QdrantFormat
    {
        public Guid Id = Guid.NewGuid();

        public float[] Vector;

        public QdrantPayload Payload;
    }
}
