namespace DataInjection.Interfaces
{
    /// <summary>
    /// Defines a contract for collecting entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to collect.</typeparam>
    public interface IEntityCollector<TEntity>
    {
        /// <summary>
        /// Asynchronously retrieves a list of entities for the specified municipality.
        /// </summary>
        /// <param name="municipality">The name of the municipality to filter entities by.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of entities.
        /// </returns>
        Task<List<TEntity>> GetEntities(string municipality);
    }
}
