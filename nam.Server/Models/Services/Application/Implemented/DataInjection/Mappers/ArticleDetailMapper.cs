using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Application.Interfaces.DataInjection;

namespace nam.Server.Models.Services.Application.Implemented.DataInjection.Mappers
{
    public class ArticleDetailMapper : IDtoMapper<ArticleDetailDto, ArticleDetail>
    {
        public ArticleDetail MapToEntity(ArticleDetailDto dto)
        {
            if (dto is null)
            {
                return new ArticleDetail
                {
                    Identifier = Guid.NewGuid(),
                    Title = string.Empty,
                    Script = string.Empty,
                    ImagePath = string.Empty,
                    UpdatedAt = DateTime.UtcNow,
                    Paragraphs = new List<Paragraph>(),
                    Themes = new List<string>(),
                    MunicipalityData = new MunicipalityForLocalStorageSetting { Name = string.Empty, LogoPath = string.Empty }
                };
            }

            // Identifier
            _ = Guid.TryParse(dto.Identifier, out Guid identifier);
            if (identifier == Guid.Empty)
                identifier = Guid.NewGuid();

            var detail = new ArticleDetail
            {
                Identifier = identifier,
                Title = dto.Title ?? string.Empty,
                Script = dto.Script ?? string.Empty,
                Region = dto.Region,
                Subtitle = dto.Subtitle,
                TimeToRead = dto.TimeToRead,
                ImagePath = dto.ImagePath ?? string.Empty,
                UpdatedAt = dto.UpdatedAt == default ? DateTime.UtcNow : dto.UpdatedAt,
                Themes = new List<string>(),
                Paragraphs = new List<Paragraph>(),
                MunicipalityData = dto.MunicipalityData is not null
                    ? dto.MunicipalityData
                    : new MunicipalityForLocalStorageSetting { Name = string.Empty, LogoPath = string.Empty }
            };

            // Themes
            if (dto.Themes != null && dto.Themes.Any())
            {
                foreach (var t in dto.Themes)
                {
                    detail.Themes.Add(t ?? string.Empty);
                }
            }

            // Paragraphs
            if (dto.Paragraphs != null && dto.Paragraphs.Any())
            {
                foreach (var p in dto.Paragraphs)
                {
                    if (p is null) continue;

                    var paragraph = new Paragraph
                    {
                        Id = p.Id != Guid.Empty ? p.Id : Guid.NewGuid(),
                        Position = p.Position,
                        Title = p.Title ?? string.Empty,
                        Script = p.Script ?? string.Empty,
                        Subtitle = p.Subtitle,
                        Region = p.Region,
                        ReferenceIdentifier = p.ReferenceIdentifier,
                        ReferenceName = p.ReferenceName,
                        ReferenceCategory = p.ReferenceCategory,
                        ReferenceImagePath = p.ReferenceImagePath,
                        ReferenceLatitude = p.ReferenceLatitude,
                        ReferenceLongitude = p.ReferenceLongitude
                    };

                    detail.Paragraphs.Add(paragraph);
                }
            }
            return detail;
        }
    }
}
