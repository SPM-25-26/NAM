namespace DataInjection.Interfaces
{
    /// <summary>
    /// Defines a contract for providing an entity of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to provide.</typeparam>
    public interface IProvider<TEntity>
    {
        /// <summary>
        /// Asynchronously retrieves an entity.
        /// </summary>
        /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the retrieved entity.
        /// </returns>
        Task<TEntity> GetEntity(CancellationToken ct = default);
    }
}
