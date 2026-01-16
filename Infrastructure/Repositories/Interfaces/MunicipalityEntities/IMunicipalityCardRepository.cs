using Domain.Entities.MunicipalityEntities;

namespace Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface IMunicipalityCardRepository : IRepository<MunicipalityCard, string>, IMunicipalityEntityRepository<MunicipalityCard, MunicipalityHomeInfo, string>
    {
    }
}
