using DataInjection.Interfaces;

namespace DataInjection.Providers
{
    public class ExternalEndpointProvider<TDto, TEntity>(IConfiguration configuration, IFetcher fetcher, IDtoMapper<TDto, TEntity> mapper, string endpoint, Dictionary<string, string?> query) : AbstractProvider<TDto, TEntity>(fetcher, mapper, endpoint, query)
    {
        public override string GetBaseUrl()
        {
            return configuration["DataInjectionApi"];
        }
    }
}
