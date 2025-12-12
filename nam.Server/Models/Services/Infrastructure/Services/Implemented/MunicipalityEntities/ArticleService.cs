using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.MunicipalityEntities
{
    public class ArticleService(IArticleRepository articleRepository) : MunicipalityEntityService<ArticleCard, ArticleDetail>(articleRepository)
    {
    }
}
