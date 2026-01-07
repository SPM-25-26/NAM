using DataInjection.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DataInjection.Core.Providers
{
    public class WebServerProvider<TDto, TEntity>(IConfiguration configuration, IFetcher fetcher, IDtoMapper<TDto, TEntity> mapper, string endpoint, Dictionary<string, string?> query) : AbstractProvider<TDto, TEntity>(fetcher, mapper, endpoint, query)
    {
        public override string GetBaseUrl()
        {
            return configuration["SERVER_HTTPS"];
        }
    }
}
