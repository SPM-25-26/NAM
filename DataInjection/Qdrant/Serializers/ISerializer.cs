namespace DataInjection.Qdrant.Serializers
{
    public interface ISerializer<TEntity>
    {
        string ToEmbeddingString(TEntity entity);
    }
}
