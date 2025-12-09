using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Mappers
{
    public class ArticleCardMapper : IDtoMapper<List<ArticleCardDto>, List<ArticleCard>>
    {
        public List<ArticleCard> MapToEntity(List<ArticleCardDto> dto)
        {
            var entities = new List<ArticleCard>();

            if (dto is null) return entities;

            foreach (var item in dto)
            {
                if (item is null) continue;

                // Try to parse incoming entityId; if invalid or empty generate a new Guid
                Guid id = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(item.entityId))
                {
                    Guid.TryParse(item.entityId, out id);
                }
                if (id == Guid.Empty)
                {
                    id = Guid.NewGuid();
                }

                var card = new ArticleCard
                {
                    EntityId = id,
                    EntityName = item.EntityName ?? string.Empty,
                    BadgeText = item.BadgeText ?? string.Empty,
                    ImagePath = item.ImagePath ?? string.Empty,
                    Address = item.Address
                };

                entities.Add(card);
            }

            return entities;
        }
    }
}
