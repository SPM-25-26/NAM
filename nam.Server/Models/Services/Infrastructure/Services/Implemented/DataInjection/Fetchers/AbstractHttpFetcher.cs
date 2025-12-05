using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Fetchers
{
    public abstract class AbstractHttpFetcher<TDto, TEntity>(HttpClient httpClient, Serilog.ILogger logger, IConfiguration Configuration, Dictionary<string, string?> query)
    {
        protected readonly HttpClient _httpClient = httpClient;
        protected readonly Serilog.ILogger _logger = logger;
        protected readonly IConfiguration _configuration = Configuration;

        public async Task<TEntity?> FetchAndMapAsync(CancellationToken cancellationToken = default)
        {
            var baseUrl = _configuration["DataInjectionApi"];
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("Configuration key 'DataInjectionApi' is not set.");

            var endpointPath = GetEndpointUrl();

            // Se l'endpoint contiene già dei parametri di percorso (es. {id}), sostituiscili
            var processedEndpoint = ReplacePathParameters(endpointPath, query);

            // Aggiungi solo i parametri di query rimanenti che non sono stati usati come parametri di percorso
            var remainingQuery = GetRemainingQueryParameters(query);
            var relativeWithQuery = remainingQuery.Any()
                ? QueryHelpers.AddQueryString(processedEndpoint, remainingQuery)
                : processedEndpoint;

            var fullUri = new Uri(new Uri(baseUrl.TrimEnd('/') + "/"), relativeWithQuery).ToString();


            try
            {
                _logger.Information($"Fetching data from {fullUri}...");

                // 1. Fetch Data
                var response = await _httpClient.GetAsync(fullUri, cancellationToken);
                response.EnsureSuccessStatusCode();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                // handle enum conversion
                options.Converters.Add(new JsonStringEnumConverter());

                // 2. Deserialize to DTO
                var dto = await response.Content.ReadFromJsonAsync<TDto>(options, cancellationToken);

                if (dto == null)
                {
                    _logger.Warning("API returned null or empty body.");
                    return default;
                }

                // 3. Map to Entity
                var entity = MapToEntity(dto);
                return entity;
            }
            catch (HttpRequestException ex)
            {
                _logger.Error(ex, $"Error fetching data from {fullUri}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred during fetch/map process.");
                throw;
            }
        }

        protected virtual string ReplacePathParameters(string endpoint, Dictionary<string, string?> query)
        {
            var result = endpoint;
            foreach (var kvp in query)
            {
                var placeholder = $"{{{kvp.Key}}}";
                if (result.Contains(placeholder) && kvp.Value != null)
                {
                    result = result.Replace(placeholder, kvp.Value);
                }
            }
            return result;
        }

        protected virtual Dictionary<string, string?> GetRemainingQueryParameters(Dictionary<string, string?> query)
        {
            var endpoint = GetEndpointUrl();
            var remaining = new Dictionary<string, string?>();

            foreach (var kvp in query)
            {
                var placeholder = $"{{{kvp.Key}}}";
                // Se il parametro non è usato come segnaposto nell'URL, aggiungilo alla query string
                if (!endpoint.Contains(placeholder))
                {
                    remaining[kvp.Key] = kvp.Value;
                }
            }

            return remaining;
        }

        protected abstract string GetEndpointUrl();

        protected abstract TEntity MapToEntity(TDto dto);
    }
}
