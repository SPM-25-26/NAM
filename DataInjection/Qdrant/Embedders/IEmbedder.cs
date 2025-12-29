namespace DataInjection.Qdrant.Embedders
{
    public interface IEmbedder
    {
        Task<float[]> GetEmbeddingAsync(string text);

        Task<List<float[]>> GetBatchEmbeddingAsync(ICollection<string> strings);
    }
}
