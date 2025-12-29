using Domain.Entities.MunicipalityEntities;

namespace Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface INatureRepository : IRepository<Nature, Guid>, IMunicipalityEntityRepository<Nature, ArtCultureNatureDetail, Guid>
    {
    }
}
