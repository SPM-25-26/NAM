using DataInjection.Core.Interfaces;
using DataInjection.SQL.DTOs;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.SQL.Mappers
{
    public class RouteCardDetailMapper : IDtoMapper<RouteDetailDto, RouteDetail>
    {
        public RouteDetail MapToEntity(RouteDetailDto dto)
        {
            if (dto is null)
            {
                return new RouteDetail
                {
                    Identifier = Guid.NewGuid()
                };
            }

            // NB: il DTO non espone Identifier: viene tipicamente riallineato dal Collector a `RouteCard.EntityId`.
            var detail = new RouteDetail
            {
                Identifier = Guid.NewGuid(),

                ImagePath = dto.ImagePath?.Trim(),
                Number = dto.Number?.Trim(),
                Name = dto.Name?.Trim(),
                Description = dto.Description?.Trim(),
                PathTheme = dto.PathTheme?.Trim(),
                TravellingMethod = dto.TravellingMethod?.Trim(),
                ShortName = dto.ShortName?.Trim(),
                OrganizationWebsite = dto.OrganizationWebsite?.Trim(),
                OrganizationEmail = dto.OrganizationEmail?.Trim(),
                OrganizationFacebook = dto.OrganizationFacebook?.Trim(),
                OrganizationInstagram = dto.OrganizationInstagram?.Trim(),
                OrganizationTelephone = dto.OrganizationTelephone?.Trim(),
                Website = dto.Website?.Trim(),
                SecurityLevel = dto.SecurityLevel?.Trim(),
                NumberOfStages = dto.NumberOfStages,
                QuantifiedPathwayPaving = dto.QuantifiedPathwayPaving,
                Duration = dto.Duration?.Trim(),
                RouteLength = dto.RouteLength?.Trim(),
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
            };

            // Gallery
            if (dto.Gallery != null && dto.Gallery.Any())
            {
                foreach (var img in dto.Gallery)
                {
                    if (!string.IsNullOrWhiteSpace(img))
                        detail.Gallery.Add(img.Trim());
                }
            }

            // Virtual tours
            if (dto.VirtualTours != null && dto.VirtualTours.Any())
            {
                foreach (var vt in dto.VirtualTours)
                {
                    if (!string.IsNullOrWhiteSpace(vt))
                        detail.VirtualTours.Add(vt.Trim());
                }
            }

            // Best when
            if (dto.BestWhen != null && dto.BestWhen.Any())
            {
                foreach (var bw in dto.BestWhen)
                {
                    if (!string.IsNullOrWhiteSpace(bw))
                        detail.BestWhen.Add(bw.Trim());
                }
            }

            // Starting point
            if (dto.StartingPoint != null)
            {
                detail.StartingPoint = new Point
                {
                    Address = string.IsNullOrWhiteSpace(dto.StartingPoint.Address) ? string.Empty : dto.StartingPoint.Address.Trim(),
                    Latitude = dto.StartingPoint.Latitude,
                    Longitude = dto.StartingPoint.Longitude
                };
            }

            // Municipality data
            if (dto.MunicipalityData != null)
            {
                detail.MunicipalityData = new MunicipalityForLocalStorageSetting
                {
                    Name = dto.MunicipalityData.Name ?? string.Empty,
                    LogoPath = dto.MunicipalityData.LogoPath ?? string.Empty
                };
            }

            // Stages -> StageMobile + relationship
            if (dto.Stages != null && dto.Stages.Any())
            {
                foreach (var s in dto.Stages)
                {
                    if (s is null) continue;

                    var stageEntity = new StageMobile
                    {
                        Category = s.Category?.Trim(),
                        PoiIdentifier = Guid.TryParse(s.PoiIdentifier, out var poiId) && poiId != Guid.Empty ? poiId : Guid.NewGuid(),
                        PoiOfficialName = s.PoiOfficialName?.Trim(),
                        PoiImagePath = s.PoiImagePath?.Trim(),
                        PoiImageThumbPath = s.PoiImageThumbPath?.Trim(),
                        Signposting = s.Signposting?.Trim(),
                        SupportService = s.SupportService?.Trim(),
                        PoiLatitude = s.PoiLatitude,
                        PoiLongitude = s.PoiLongitude,
                        PoiAddress = s.PoiAddress?.Trim(),
                        Name = s.Name?.Trim(),
                        Number = s.Number,
                        Description = s.Description?.Trim()
                    };

                    var rel = new StageMobileRelationship<RouteDetail>
                    {
                        StageMobile = stageEntity,
                        RelatedEntity = detail
                    };

                    detail.Stages.Add(rel);
                    stageEntity.RouteRelations.Add(rel);
                }
            }

            // StagesPoi -> FeatureCard + relationship
            var poiCards = dto.StagesPoi?
                .Where(p => p is not null)
                .Select(p => new FeatureCard
                {
                    EntityId = Guid.TryParse(p!.EntityId, out var id) && id != Guid.Empty ? id : Guid.NewGuid(),
                    Title = p.Title ?? default,
                    Category = p.Category ?? default,
                    ImagePath = p.ImagePath ?? default,
                    ExtraInfo = p.ExtraInfo ?? default,
                })
                .ToList();

            if (poiCards != null)
            {
                foreach (var fc in poiCards)
                {
                    var rel = new FeatureCardRelationship<RouteDetail> { FeatureCard = fc, RelatedEntity = detail };
                    detail.StagesPoi.Add(rel);
                    fc.RouteRelations.Add(rel);
                }
            }

            return detail;
        }
    }
}