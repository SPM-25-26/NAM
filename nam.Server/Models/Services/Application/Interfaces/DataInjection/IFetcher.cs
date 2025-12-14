namespace nam.Server.Models.Services.Application.Interfaces.DataInjection
{
    public interface IFetcher
    {
        public Task<TDto> Fetch<TDto>(string endpointUrl, Dictionary<string, string?> query, CancellationToken cancellationToken = default);
    }
}
