
using Domain.Entities.MunicipalityEntities;

namespace Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface IArtCultureRepository : IRepository<ArtCultureNatureCard, Guid>, IMunicipalityEntityRepository<ArtCultureNatureCard, ArtCultureNatureDetail, Guid>
    {
    }
}
