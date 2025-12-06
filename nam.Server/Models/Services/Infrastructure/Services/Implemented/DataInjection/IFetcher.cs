namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection
{
    public interface IFetcher
    {
        public Task<TDto> Fetch<TDto>(string endpointUrl, Dictionary<string, string?> query, CancellationToken cancellationToken = default);
    }
}
