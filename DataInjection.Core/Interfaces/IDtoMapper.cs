namespace DataInjection.Interfaces
{
    public interface IDtoMapper<TDto, TEntity>
    {
        TEntity MapToEntity(TDto dto);
    }
}
