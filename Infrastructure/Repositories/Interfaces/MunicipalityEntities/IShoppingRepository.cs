using Domain.Entities.MunicipalityEntities;


namespace Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface IShoppingRepository : IRepository<ShoppingCard, Guid>, IMunicipalityEntityRepository<ShoppingCard, ShoppingCardDetail, Guid>
    {
    }
}
