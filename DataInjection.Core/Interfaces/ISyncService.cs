namespace DataInjection.Core.Interfaces
{
    /// <summary>
    /// Defines a contract for a synchronization service that executes synchronization logic for entities.
    /// </summary>
    public interface ISyncService
    {
        /// <summary>
        /// Asynchronously executes the synchronization process for the specified entity type using the provided entity collector.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity to synchronize.</typeparam>
        /// <param name="entityCollector">The entity collector used to retrieve entities for synchronization.</param>
        /// <returns>
        /// A task that represents the asynchronous synchronization operation.
        /// </returns>
        Task ExecuteSyncAsync<TEntity>(IEntityCollector<TEntity> entityCollector);
    }
}
