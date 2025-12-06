namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection
{
    public interface IDtoMapper<TDto, TEntity>
    {
        TEntity MapToEntity(TDto dto);
    }
}
