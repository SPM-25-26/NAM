namespace DataInjection.Interfaces
{
    public interface IFetcher
    {
        public Task<TDto> Fetch<TDto>(string baseUrl, string endpointUrl, Dictionary<string, string?> query, CancellationToken cancellationToken = default);
    }
}
