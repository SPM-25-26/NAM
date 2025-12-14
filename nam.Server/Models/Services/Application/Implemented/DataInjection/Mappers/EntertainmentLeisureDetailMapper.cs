using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Application.Interfaces.DataInjection;

namespace nam.Server.Models.Services.Application.Implemented.DataInjection.Mappers
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
            {
                identifier = Guid.NewGuid();
            }


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
            if (dto.Neighbors != null && dto.Neighbors.Count != 0)
            {
                foreach (var n in dto.Neighbors)
                {
                    detail.Neighbors.Add(new FeatureCard
                    {
                        EntityId = n?.EntityId ?? string.Empty,
                        Title = n?.Title ?? string.Empty,
                        Category = n?.Category ?? MobileCategory.EntertainmentLeisure,
                        ImagePath = n?.ImagePath ?? string.Empty,
                        ExtraInfo = n?.ExtraInfo
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

            // Associated services
            if (dto.AssociatedServices != null && dto.AssociatedServices.Any())
            {
                foreach (var a in dto.AssociatedServices)
                {
                    detail.AssociatedServices.Add(new AssociatedService
                    {
                        Identifier = a?.Identifier ?? string.Empty,
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
