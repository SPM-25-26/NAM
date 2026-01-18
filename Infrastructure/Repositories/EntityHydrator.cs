using Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repositories
{
    public class EntityHydrator(IServiceScopeFactory scopeFactory)
    {
        public async Task<string?> HydrateAsync(string endpointName, string id, CancellationToken ct = default)
        {
            using var scope = scopeFactory.CreateScope();
            var sources = scope.ServiceProvider.GetRequiredService<IEnumerable<IEntitySource>>();
            var source = sources.FirstOrDefault(s => s.EntityName == endpointName);

            if (source != null)
                return await source.GetContentAsync(id, ct);

            return null;
        }
    }
}
