namespace nam.Server.Models.Services.Application.Interfaces.DataInjection
{
    public interface IDtoMapper<TDto, TEntity>
    {
        TEntity MapToEntity(TDto dto);
    }
}
