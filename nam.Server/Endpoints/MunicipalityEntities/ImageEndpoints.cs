using Microsoft.AspNetCore.Mvc;

namespace nam.Server.Endpoints.MunicipalityEntities
{
    public class ImageEndpoints
    {
        private static Serilog.ILogger? _logger;

        public static void ConfigureLogger(Serilog.ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public static async Task<IResult> GetExternalImage(
            [FromServices] IConfiguration _configuration,
            [FromServices] IHttpClientFactory _httpClientFactory,
            [FromQuery] string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return TypedResults.BadRequest("Image path is required.");

            var baseUrl = _configuration["ExternalImageBaseUrl"];
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                _logger?.Warning("ExternalImageBaseUrl not configured.");
                return TypedResults.BadRequest("ExternalImageBaseUrl not configured.");
            }

            if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out var baseUri))
            {
                _logger?.Warning("ExternalImageBaseUrl is not a valid absolute URI: {BaseUrl}", baseUrl);
                return TypedResults.BadRequest("Invalid ExternalImageBaseUrl configuration.");
            }

            Uri imageUri;
            try
            {
                imageUri = new Uri(baseUri, imagePath);
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Invalid image path when building URI: {ImagePath}", imagePath);
                return TypedResults.BadRequest("Invalid image path.");
            }

            var client = _httpClientFactory.CreateClient();

            try
            {
                var response = await client.GetAsync(imageUri, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    _logger?.Information("Image not found at {Url} (status {StatusCode})", imageUri, response.StatusCode);
                    return TypedResults.NotFound();
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";

                return Results.File(stream, contentType);
            }
            catch (HttpRequestException ex)
            {
                _logger?.Error(ex, "HTTP error fetching image from {Url}", imageUri);
                return TypedResults.Problem("Failed to fetch image from external source.");
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Unexpected error fetching image from {Url}", imageUri);
                return TypedResults.Problem("Unexpected error.");
            }
        }
    }
}
