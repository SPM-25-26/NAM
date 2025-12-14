using nam.Server.Models.Entities.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface IArtCultureRepository : IRepository<ArtCultureNatureCard, Guid>, IMunicipalityEntityRepository<ArtCultureNatureCard, ArtCultureNatureDetail, Guid>
    {
    }
}
