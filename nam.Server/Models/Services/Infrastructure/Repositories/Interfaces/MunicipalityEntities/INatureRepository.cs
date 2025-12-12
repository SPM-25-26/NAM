using nam.Server.Models.Entities.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface INatureRepository : IRepository<Nature, Guid>, IMunicipalityEntityRepository<Nature, ArtCultureNatureDetail, Guid>
    {
    }
}
