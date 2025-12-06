using Microsoft.AspNetCore.WebUtilities;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Fetchers
{
    public class HttpFetcherService(IHttpClientFactory httpClientFactory, Serilog.ILogger logger, IConfiguration configuration) : IFetcher
    {
        protected readonly HttpClient _httpClient = httpClientFactory.CreateClient();
        protected readonly Serilog.ILogger _logger = logger;
        protected readonly IConfiguration _configuration = configuration;

        public async Task<TDto> Fetch<TDto>(string endpointUrl, Dictionary<string, string?> query, CancellationToken cancellationToken = default)
        {
            var baseUrl = _configuration["DataInjectionApi"];
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("Configuration key 'DataInjectionApi' is not set.");

            // Se l'endpoint contiene già dei parametri di percorso (es. {id}), sostituiscili
            var processedEndpoint = ReplacePathParameters(endpointUrl, query);

            // Aggiungi solo i parametri di query rimanenti che non sono stati usati come parametri di percorso
            var remainingQuery = GetRemainingQueryParameters(endpointUrl, query);
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

                // Leggi il contenuto della risposta e loggalo
                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                // handle enum conversion
                options.Converters.Add(new JsonStringEnumConverter());

                // 2. Deserialize to DTO a partire dalla stringa letta
                if (string.IsNullOrWhiteSpace(content))
                {
                    _logger.Warning("API returned null or empty body.");
                    return default;
                }

                var dto = JsonSerializer.Deserialize<TDto>(content, options);

                if (dto is null)
                {
                    _logger.Warning("Deserialization produced null DTO.");
                    return default;
                }
                return dto;
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

        protected virtual Dictionary<string, string?> GetRemainingQueryParameters(string endpoint, Dictionary<string, string?> query)
        {
            var remaining = new Dictionary<string, string?>();

            foreach (var kvp in query)
            {
                var placeholder = $"{{{kvp.Key}}}";
                // Se il parametro non è usato come segnaposto nell'URL, aggiungilo alla query string
                if (!endpoint.Contains(placeholder))
                    remaining[kvp.Key] = kvp.Value;
            }

            return remaining;
        }
    }
}
