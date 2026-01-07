using DataInjection.Core.Interfaces;

namespace DataInjection.Core.Providers
{
    /// <summary>
    /// Abstract base provider for fetching and mapping data transfer objects (DTOs) to entities.
    /// </summary>
    /// <typeparam name="TDto">The type of the data transfer object.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="fetcher">The fetcher responsible for retrieving DTOs from a data source.</param>
    /// <param name="mapper">The mapper used to convert DTOs to entities.</param>
    /// <param name="endpoint">The endpoint to fetch data from.</param>
    /// <param name="query">The query parameters to use when fetching data.</param>
    public abstract class AbstractProvider<TDto, TEntity>(
        IFetcher fetcher,
        IDtoMapper<TDto, TEntity> mapper,
        string endpoint,
        Dictionary<string, string?> query
    ) : IProvider<TEntity>
    {
        private readonly Dictionary<string, string?> _query = query;

        /// <summary>
        /// Gets the query parameters used for data fetching.
        /// </summary>
        public IDictionary<string, string?> Query => _query;

        private readonly string _endpoint = endpoint;

        /// <summary>
        /// Gets the base URL for the data source.
        /// </summary>
        /// <returns>The base URL as a string.</returns>
        public abstract string GetBaseUrl();

        /// <summary>
        /// Asynchronously fetches the DTOs from the data source and maps them to an entity.
        /// </summary>
        /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>The mapped entity.</returns>
        public async Task<TEntity> GetEntity(CancellationToken ct = default)
        {
            var dtos = await fetcher.Fetch<TDto>(GetBaseUrl(), _endpoint, _query, ct);
            return mapper.MapToEntity(dtos);
        }
    }
}