namespace DataInjection.Core.Interfaces
{
    /// <summary>
    /// Defines a contract for fetching data transfer objects (DTOs) from a data source.
    /// </summary>
    public interface IFetcher
    {
        /// <summary>
        /// Asynchronously fetches a data transfer object (DTO) from the specified endpoint and query parameters.
        /// </summary>
        /// <typeparam name="TDto">The type of the data transfer object to fetch.</typeparam>
        /// <param name="baseUrl">The base URL of the data source.</param>
        /// <param name="endpointUrl">The endpoint URL to fetch data from.</param>
        /// <param name="query">The query parameters to include in the request.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous fetch operation. The task result contains the fetched DTO.
        /// </returns>
        public Task<TDto> Fetch<TDto>(string baseUrl, string endpointUrl, Dictionary<string, string?> query, CancellationToken cancellationToken = default);
    }
}
