using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Mappers
{
    public class ArticleCardMapper : IDtoMapper<List<ArticleCardDto>, List<ArticleCard>>
    {
        public List<ArticleCard> MapToEntity(List<ArticleCardDto> dto)
        {
            if (dto == null || dto.Count == 0)
                return [];
            return dto.Where(dto => dto is not null)
                      .Select(item => new ArticleCard
                      {
                          EntityId = Guid.TryParse(item.entityId, out Guid id) && id != Guid.Empty ? id : Guid.NewGuid(),
                          EntityName = item.EntityName ?? string.Empty,
                          BadgeText = item.BadgeText ?? string.Empty,
                          ImagePath = item.ImagePath ?? string.Empty,
                          Address = item.Address
                      }).ToList();
        }
    }
}
