namespace DataInjection.Qdrant.Embedders
{
    public interface IEmbedder
    {
        Task<float[]> GetEmbeddingAsync(string text, int outputDimensionality);

        Task<List<float[]>> GetBatchEmbeddingAsync(ICollection<string> strings, int outputDimensionality);
    }
}
