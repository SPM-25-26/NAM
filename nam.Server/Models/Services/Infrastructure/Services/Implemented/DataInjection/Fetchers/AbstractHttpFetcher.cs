using Microsoft.AspNetCore.WebUtilities;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Fetchers
{
    public abstract class AbstractHttpFetcher<TDto, TEntity>(HttpClient httpClient, ILogger logger, IConfiguration Configuration, Dictionary<string, string?> query)
    {
        protected readonly HttpClient _httpClient = httpClient;
        protected readonly ILogger _logger = logger;
        protected readonly IConfiguration _configuration = Configuration;

        public async Task<TEntity?> FetchAndMapAsync(CancellationToken cancellationToken = default)
        {
            var baseUrl = _configuration["DataInjectionApi"];
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("Configuration key 'DataInjectionApi' is not set.");

            var endpointPath = GetEndpointUrl();
            var relativeWithQuery = QueryHelpers.AddQueryString(endpointPath, query);

            var fullUri = new Uri(new Uri(baseUrl.TrimEnd('/') + "/"), relativeWithQuery).ToString();


            try
            {
                _logger.LogInformation($"Fetching data from {fullUri}...");

                // 1. Fetch Data
                var response = await _httpClient.GetAsync(fullUri, cancellationToken);
                response.EnsureSuccessStatusCode();

                // 2. Deserialize to DTO
                var dto = await response.Content.ReadFromJsonAsync<TDto>(cancellationToken: cancellationToken);

                if (dto == null)
                {
                    _logger.LogWarning("API returned null or empty body.");
                    return default;
                }

                // 3. Map to Entity
                var entity = MapToEntity(dto);
                return entity;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"Error fetching data from {fullUri}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during fetch/map process.");
                throw;
            }
        }

        protected abstract string GetEndpointUrl();

        protected abstract TEntity MapToEntity(TDto dto);
    }
}
