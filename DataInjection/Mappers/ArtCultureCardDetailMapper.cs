using DataInjection.Interfaces;
using Domain.DTOs.MunicipalityInjection;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.Mappers
{
    public class ArtCultureCardDetailMapper : IDtoMapper<ArtCultureNatureDetailDto, ArtCultureNatureDetail>
    {
        public ArtCultureNatureDetail MapToEntity(ArtCultureNatureDetailDto dto)
        {
            if (dto is null)
            {
                return new ArtCultureNatureDetail
                {
                    Identifier = Guid.NewGuid()
                };
            }

            // Basic scalar mapping (use empty strings for nulls to satisfy required fields)
            var detail = new ArtCultureNatureDetail
            {
                Identifier = Guid.TryParse(dto.Identifier, out var identifier) ? identifier : Guid.NewGuid(),
                OfficialName = dto.OfficialName ?? string.Empty,
                PrimaryImagePath = dto.PrimaryImagePath ?? string.Empty,
                FullAddress = dto.FullAddress ?? string.Empty,
                Type = dto.Type ?? string.Empty,
                SubjectDiscipline = dto.SubjectDiscipline ?? string.Empty,
                Description = dto.Description ?? string.Empty,
                Email = dto.Email ?? string.Empty,
                Telephone = dto.Telephone ?? string.Empty,
                Website = dto.Website ?? string.Empty,
                Instagram = dto.Instagram ?? string.Empty,
                Facebook = dto.Facebook ?? string.Empty,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
            };

            // Collections mapping
            if (dto.Services != null && dto.Services.Count != 0)
            {
                foreach (var s in dto.Services)
                {
                    detail.Services.Add(new CulturalSiteService
                    {
                        Name = s?.Name ?? string.Empty,
                        Description = s?.Description ?? string.Empty
                    });
                }
            }

            if (dto.CulturalProjects != null && dto.CulturalProjects.Any())
            {
                foreach (var p in dto.CulturalProjects)
                {
                    detail.CulturalProjects.Add(new CulturalProject
                    {
                        Name = p?.Name ?? string.Empty,
                        Url = p?.Url ?? string.Empty
                    });
                }
            }

            if (dto.Catalogues != null && dto.Catalogues.Any())
            {
                foreach (var c in dto.Catalogues)
                {
                    detail.Catalogues.Add(new Catalogue
                    {
                        Name = c?.Name ?? string.Empty,
                        WebsiteUrl = c?.WebsiteUrl ?? string.Empty,
                        Description = c?.Description ?? string.Empty
                    });
                }
            }

            if (dto.CreativeWorks != null && dto.CreativeWorks.Any())
            {
                foreach (var cw in dto.CreativeWorks)
                {
                    detail.CreativeWorks.Add(new CreativeWorkMobile
                    {
                        Type = cw?.Type ?? string.Empty,
                        Url = cw?.Url ?? string.Empty
                    });
                }
            }

            // Simple string collections
            if (dto.Gallery != null && dto.Gallery.Any())
            {
                foreach (var img in dto.Gallery)
                {
                    detail.Gallery.Add(img ?? string.Empty);
                }
            }

            if (dto.VirtualTours != null && dto.VirtualTours.Any())
            {
                foreach (var vt in dto.VirtualTours)
                {
                    detail.VirtualTours.Add(vt ?? string.Empty);
                }
            }


            // Neighbors -> FeatureCard
            var neigh = dto.Neighbors?
            .Where(n => n is not null)
            .Select(n =>
             new FeatureCard
             {
                 EntityId = Guid.TryParse(n.EntityId, out var neighId) ? neighId : Guid.NewGuid(),
                 Title = n.Title ?? default,
                 Category = n.Category ?? default,
                 ImagePath = n?.ImagePath ?? default,
                 ExtraInfo = n?.ExtraInfo ?? default,
             })
            .ToList();

            foreach (var n in neigh)
            {
                var fcr = new FeatureCardRelationship<ArtCultureNatureDetail> { FeatureCard = n, RelatedEntity = detail };
                detail.Neighbors.Add(fcr);
                n.ArtCultureRelations.Add(fcr);
            }

            //detail.Neighbors = neigh is not null ? neigh : [];
            // Associated services
            if (dto.AssociatedServices != null && dto.AssociatedServices.Any())
            {
                foreach (var a in dto.AssociatedServices)
                {
                    Guid.TryParse(a.Identifier, out var serviceId);
                    if (serviceId == Guid.Empty)
                        serviceId = Guid.NewGuid();
                    detail.AssociatedServices.Add(new AssociatedService
                    {
                        Identifier = serviceId,
                        Name = a?.Name ?? string.Empty,
                        ImagePath = a?.ImagePath ?? string.Empty
                    });
                }
            }

            // NearestCarPark
            if (dto.NearestCarPark != null)
            {
                detail.NearestCarPark = new NearestCarPark
                {
                    Latitude = dto.NearestCarPark.Latitude,
                    Longitude = dto.NearestCarPark.Longitude,
                    Address = dto.NearestCarPark.Address ?? string.Empty,
                    Distance = dto.NearestCarPark.Distance
                };
            }

            // Site
            if (dto.Site != null)
            {
                Guid.TryParse(dto.Site.Identifier, out var siteId);
                if (siteId == Guid.Empty)
                    siteId = Guid.NewGuid();
                detail.Site = new SiteCard
                {
                    Identifier = siteId,
                    OfficialName = dto.Site.OfficialName ?? string.Empty,
                    ImagePath = dto.Site.ImagePath ?? string.Empty,
                    Category = dto.Site.Category ?? string.Empty
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

            return detail;
        }
    }
}
