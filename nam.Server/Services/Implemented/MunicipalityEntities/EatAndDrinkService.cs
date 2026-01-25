using Domain.Entities.MunicipalityEntities;
using Infrastructure.UnitOfWork;

namespace nam.Server.Services.Implemented.MunicipalityEntities
{
    public class EatAndDrinkService(IUnitOfWork unitOfWork) : MunicipalityEntityService<EatAndDrinkCard, EatAndDrinkDetail>(unitOfWork.EatAndDrink)
    {
    }
}
