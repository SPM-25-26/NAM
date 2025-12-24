using Domain.Entities.MunicipalityEntities;
using Infrastructure.UnitOfWork;

namespace nam.Server.Models.Services.Application.Implemented.MunicipalityEntities
{
    public class ArticleService(IUnitOfWork unitOfWork) : MunicipalityEntityService<ArticleCard, ArticleDetail>(unitOfWork.Article)
    {
    }
}
