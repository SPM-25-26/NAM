using nam.Server.Models.Entities.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface IMunicipalityCardRepository : IRepository<MunicipalityCard, string>, IMunicipalityEntityRepository<MunicipalityCard, MunicipalityHomeInfo, string>
    {
    }
}
