using nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Providers
{
    public class BaseProvider<TDto, TEntity>(IFetcher fetcher, IDtoMapper<TDto, TEntity> mapper, string endpoint, Dictionary<string, string?> query) : IProvider<TEntity>
    {
        private readonly Dictionary<string, string?> _query = query;
        public IDictionary<string, string?> Query => _query;

        private readonly string _endpoint = endpoint;

        public async Task<TEntity> GetEntity(CancellationToken ct = default)
        {
            var dtos = await fetcher.Fetch<TDto>(_endpoint, _query, ct);
            return mapper.MapToEntity(dtos);
        }
    }
}