using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Interfaces;

namespace nam.Server.Models.Services.Application.Implemented.MunicipalityEntities
{
    public class ArticleService(IUnitOfWork unitOfWork) : MunicipalityEntityService<ArticleCard, ArticleDetail>(unitOfWork.Article)
    {
    }
}
