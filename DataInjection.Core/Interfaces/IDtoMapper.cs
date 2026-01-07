namespace DataInjection.Core.Interfaces
{
    /// <summary>
    /// Defines a contract for mapping a data transfer object (DTO) to an entity.
    /// </summary>
    /// <typeparam name="TDto">The type of the data transfer object.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDtoMapper<TDto, TEntity>
    {
        /// <summary>
        /// Maps the specified data transfer object (DTO) to an entity.
        /// </summary>
        /// <param name="dto">The data transfer object to map.</param>
        /// <returns>The mapped entity.</returns>
        TEntity MapToEntity(TDto dto);
    }
}