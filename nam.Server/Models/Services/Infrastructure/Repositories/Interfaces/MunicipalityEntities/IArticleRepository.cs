using nam.Server.Models.Entities.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface IArticleRepository : IRepository<ArticleCard, Guid>, IMunicipalityEntityRepository<ArticleCard, ArticleDetail, Guid>
    {
    }
}
