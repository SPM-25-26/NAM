using Domain.Entities.MunicipalityEntities;


namespace Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface ISleepRepository : IRepository<SleepCard, Guid>, IMunicipalityEntityRepository<SleepCard, SleepCardDetail, Guid>
    {
    }
}
