using DataInjection.Core.Interfaces;
using DataInjection.SQL.DTOs;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.SQL.Mappers
{
    public class ServiceCardDetailMapper : IDtoMapper<ServiceCardDetailDto, ServiceDetail>
    {
        public ServiceDetail MapToEntity(ServiceCardDetailDto dto)
        {
            if (dto is null)
            {
                return new ServiceDetail
                {
                    Identifier = Guid.NewGuid()
                };
            }

            // Identifier
            var identifier = Guid.TryParse(dto.Identifier, out var id) && id != Guid.Empty ? id : Guid.NewGuid();

            // Owned/embedded objects
            OpeningHoursSpecification? openingHours = null;
            if (dto.OpeningHours != null)
            {
                openingHours = new OpeningHoursSpecification
                {
                    Opens = dto.OpeningHours.Opens,
                    Closes = dto.OpeningHours.Closes,
                    Description = dto.OpeningHours.Description?.Trim(),
                    Day = dto.OpeningHours.Day,
                    AdmissionType = dto.OpeningHours.AdmissionType != null
                        ? new AdmissionType
                        {
                            Name = dto.OpeningHours.AdmissionType.Name,
                            Description = dto.OpeningHours.AdmissionType.Description?.Trim()
                        }
                        : new AdmissionType
                        {
                            Name = null,
                            Description = string.Empty
                        },
                    TimeInterval = dto.OpeningHours.TimeInterval != null
                        ? new TimeInterval
                        {
                            Date = dto.OpeningHours.TimeInterval.Date,
                            StartDate = dto.OpeningHours.TimeInterval.StartDate,
                            EndDate = dto.OpeningHours.TimeInterval.EndDate
                        }
                        : new TimeInterval
                        {
                            Date = null,
                            StartDate = null,
                            EndDate = null
                        },
                };
            }

            TemporaryClosure? temporaryClosure = null;
            if (dto.TemporaryClosure != null)
            {
                temporaryClosure = new TemporaryClosure
                {
                    ReasonForClosure = dto.TemporaryClosure.ReasonForClosure.Trim(),
                    Opens = dto.TemporaryClosure.Opens,
                    Closes = dto.TemporaryClosure.Closes,
                    Description = dto.TemporaryClosure.Description?.Trim(),
                    Day = dto.TemporaryClosure.Day,
                    TimeInterval = dto.TemporaryClosure.TimeInterval != null
                        ? new TimeInterval
                        {
                            Date = dto.TemporaryClosure.TimeInterval.Date,
                            StartDate = dto.TemporaryClosure.TimeInterval.StartDate,
                            EndDate = dto.TemporaryClosure.TimeInterval.EndDate
                        }
                        : new TimeInterval
                        {
                            Date = null,
                            StartDate = null,
                            EndDate = null
                        },
                };
            }

            Booking? booking = null;
            if (dto.Booking != null)
            {
                booking = new Booking
                {
                    Name = dto.Booking.Name,
                    Description = dto.Booking.Description?.Trim(),
                    TimeIntervalDto = dto.Booking.TimeIntervalDto != null
                        ? new TimeInterval
                        {
                            Date = dto.Booking.TimeIntervalDto.Date,
                            StartDate = dto.Booking.TimeIntervalDto.StartDate,
                            EndDate = dto.Booking.TimeIntervalDto.EndDate
                        }
                        : new TimeInterval
                        {
                            Date = null,
                            StartDate = null,
                            EndDate = null
                        },
                };
            }

            var detail = new ServiceDetail
            {
                Identifier = identifier,
                Name = dto.Name?.Trim(),
                Address = dto.Address?.Trim(),
                Description = dto.Description?.Trim(),

                SpacesForDisabled = dto.SpacesForDisabled,
                PayingParkingSpaces = dto.PayingParkingSpaces,
                AvailableParkingSpaces = dto.AvailableParkingSpaces,
                PostiAutoVenduti = dto.PostiAutoVenduti,
                TotalNumberOfCarParkSpaces = dto.TotalNumberOfCarParkSpaces,

                Latitude = dto.Latitude,
                Longitude = dto.Longitude,

                Typology = dto.Typology?.Trim(),
                PrimaryImage = dto.PrimaryImage?.Trim(),

                Email = dto.Email?.Trim(),
                Telephone = dto.Telephone?.Trim(),
                Website = dto.Website?.Trim(),
                Instagram = dto.Instagram?.Trim(),
                Facebook = dto.Facebook?.Trim(),

                Price = dto.Price?.Trim(),
                ReservationUrl = dto.ReservationUrl?.Trim(),

                OpeningHours = openingHours,
                TemporaryClosure = temporaryClosure,
                Booking = booking
            };

            // Gallery (primitive collection)
            if (dto.Gallery != null && dto.Gallery.Any())
            {
                foreach (var img in dto.Gallery)
                {
                    if (!string.IsNullOrWhiteSpace(img))
                        detail.Gallery.Add(img.Trim());
                }
            }

            // NearestCarPark (entity)
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

            // MunicipalityData (entity)
            if (dto.MunicipalityData != null)
            {
                detail.MunicipalityData = new MunicipalityForLocalStorageSetting
                {
                    Name = dto.MunicipalityData.Name ?? string.Empty,
                    LogoPath = dto.MunicipalityData.LogoPath ?? string.Empty
                };
            }

            // Neighbors -> FeatureCard relationship
            var neigh = dto.Neighbors?
                .Where(n => n is not null)
                .Select(n => new FeatureCard
                {
                    EntityId = Guid.TryParse(n!.EntityId, out var neighId) && neighId != Guid.Empty ? neighId : Guid.NewGuid(),
                    Title = n.Title ?? default,
                    Category = n.Category ?? default,
                    ImagePath = n.ImagePath ?? default,
                    ExtraInfo = n.ExtraInfo ?? default,
                })
                .ToList();

            if (neigh != null)
            {
                foreach (var n in neigh)
                {
                    var fcr = new FeatureCardRelationship<ServiceDetail> { FeatureCard = n, RelatedEntity = detail };
                    detail.Neighbors.Add(fcr);
                    n.ServiceRelations.Add(fcr); // NB: se vuoi, aggiungi una lista dedicata in FeatureCard (es. ServiceRelations) per evitare di riusare RouteRelations.
                }
            }

            // AssociatedServices (entity collection)
            if (dto.AssociatedServices != null && dto.AssociatedServices.Any())
            {
                foreach (var a in dto.AssociatedServices)
                {
                    if (a is null) continue;

                    Guid.TryParse(a.Identifier, out var serviceId);
                    if (serviceId == Guid.Empty)
                        serviceId = Guid.NewGuid();

                    detail.AssociatedServices.Add(new AssociatedService
                    {
                        Identifier = serviceId,
                        Name = a.Name ?? string.Empty,
                        ImagePath = a.ImagePath ?? string.Empty
                    });
                }
            }

            // Locations -> ServiceLocation relationship
            if (dto.Locations != null && dto.Locations.Any())
            {
                foreach (var l in dto.Locations)
                {
                    if (l is null) continue;

                    var location = new ServiceLocation
                    {
                        Identifier = l.Identifier,
                        OfficialName = l.OfficialName,
                        ImagePath = l.ImagePath,
                        Category = l.Category
                    };

                    var rel = new ServiceLocationRelationship<ServiceDetail>
                    {
                        ServiceLocation = location,
                        RelatedEntity = detail
                    };

                    detail.Locations!.Add(rel);
                }
            }

            return detail;
        }
    }
}