namespace nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection
{
    public interface IDtoMapper<TDto, TEntity>
    {
        TEntity MapToEntity(TDto dto);
    }
}
