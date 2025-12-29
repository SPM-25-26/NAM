using DataInjection.DTOs;
using DataInjection.Interfaces;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.Mappers
{
    public class EntertainmentLeisureDetailMapper : IDtoMapper<EntertainmentLeisureDetailDto, EntertainmentLeisureDetail>
    {
        public EntertainmentLeisureDetail MapToEntity(EntertainmentLeisureDetailDto dto)
        {
            if (dto is null)
            {
                return new EntertainmentLeisureDetail
                {
                    Identifier = Guid.NewGuid()
                };
            }

            Guid.TryParse(dto.Identifier, out Guid identifier);
            if (identifier == Guid.Empty)
                identifier = Guid.NewGuid();


            // Basic scalar mapping
            var detail = new EntertainmentLeisureDetail
            {
                Identifier = identifier,
                OfficialName = dto.OfficialName ?? string.Empty,
                PrimaryImagePath = dto.PrimaryImage ?? string.Empty,
                FullAddress = dto.Address ?? string.Empty,
                Category = dto.Category ?? string.Empty,
                Description = dto.Description ?? string.Empty,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
            };

            // Gallery
            if (dto.Gallery != null && dto.Gallery.Any())
            {
                foreach (var img in dto.Gallery)
                {
                    detail.Gallery.Add(img ?? string.Empty);
                }
            }

            // Virtual Tours
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
                var fcr = new FeatureCardRelationship<EntertainmentLeisureDetail> { FeatureCard = n, RelatedEntity = detail };
                detail.Neighbors.Add(fcr);
                n.EntertainmentLeisureRelations.Add(fcr);
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
