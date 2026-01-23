using Domain.Entities.MunicipalityEntities;

namespace Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface IEatAndDrinkRepository : IRepository<EatAndDrinkCard, Guid>, IMunicipalityEntityRepository<EatAndDrinkCard, EatAndDrinkDetail, Guid>
    {
    }
}
