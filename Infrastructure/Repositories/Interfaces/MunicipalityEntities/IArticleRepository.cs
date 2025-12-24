
using Domain.Entities.MunicipalityEntities;

namespace Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface IArticleRepository : IRepository<ArticleCard, Guid>, IMunicipalityEntityRepository<ArticleCard, ArticleDetail, Guid>
    {
    }
}
