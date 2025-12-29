namespace DataInjection.Qdrant.Data
{
    public record QdrantPayload
    {
        public string apiEndpoint;
        public Dictionary<string, string> apiQuery;
        public required Location Location;
    }
}
