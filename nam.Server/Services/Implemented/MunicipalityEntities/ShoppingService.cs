using Domain.Entities.MunicipalityEntities;
using Infrastructure.UnitOfWork;

namespace nam.Server.Services.Implemented.MunicipalityEntities
{
    public class ShoppingService(IUnitOfWork unitOfWork) : MunicipalityEntityService<ShoppingCard, ShoppingCardDetail>(unitOfWork.Shopping)
    {
    }
}
