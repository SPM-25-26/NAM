using DataInjection.Interfaces;

namespace DataInjection.Providers
{
    public abstract class AbstractProvider<TDto, TEntity>(IFetcher fetcher, IDtoMapper<TDto, TEntity> mapper, string endpoint, Dictionary<string, string?> query) : IProvider<TEntity>
    {
        private readonly Dictionary<string, string?> _query = query;
        public IDictionary<string, string?> Query => _query;

        private readonly string _endpoint = endpoint;

        public abstract string GetBaseUrl();

        public async Task<TEntity> GetEntity(CancellationToken ct = default)
        {
            var dtos = await fetcher.Fetch<TDto>(GetBaseUrl(), _endpoint, _query, ct);
            return mapper.MapToEntity(dtos);
        }
    }
}