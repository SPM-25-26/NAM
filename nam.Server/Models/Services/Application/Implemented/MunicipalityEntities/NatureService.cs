using Domain.Entities.MunicipalityEntities;
using Infrastructure.UnitOfWork;

namespace nam.Server.Models.Services.Application.Implemented.MunicipalityEntities
{
    public class NatureService(IUnitOfWork unitOfWork) : MunicipalityEntityService<Nature, ArtCultureNatureDetail>(unitOfWork.Nature)
    {
    }
}
