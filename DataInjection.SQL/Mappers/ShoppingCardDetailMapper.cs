using DataInjection.Core.Interfaces;
using DataInjection.SQL.DTOs;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.SQL.Mappers
{
    public class ShoppingCardDetailMapper : IDtoMapper<ShoppingCardDetailDto, ShoppingCardDetail>
    {
        public ShoppingCardDetail MapToEntity(ShoppingCardDetailDto dto)
        {
            if (dto is null)
            {
                return new ShoppingCardDetail
                {
                    Identifier = Guid.NewGuid()
                };
            }

            // Map NearestCarPark (entity)
            NearestCarPark? nearestCarPark = null;
            if (dto.NearestCarPark != null)
            {
                nearestCarPark = new NearestCarPark
                {
                    Latitude = dto.NearestCarPark.Latitude,
                    Longitude = dto.NearestCarPark.Longitude,
                    Address = dto.NearestCarPark.Address?.Trim(),
                    Distance = dto.NearestCarPark.Distance
                };
            }

            // Map Owner (owned) - crea solo se ha almeno un campo utile
            Owner? owner = null;
            if (dto.Owner != null && !string.IsNullOrWhiteSpace(dto.Owner.TaxCode) )
            {
                owner = new Owner
                {
                    TaxCode = dto.Owner.TaxCode?.Trim(),
                    LegalName = dto.Owner.LegalName?.Trim(),
                    WebSite = dto.Owner.WebSite?.Trim()
                };
            }

            // Map MunicipalityData (entity)
            MunicipalityForLocalStorageSetting? municipality = null;
            if (dto.MunicipalityData != null)
            {
                municipality = new MunicipalityForLocalStorageSetting
                {
                    Name = dto.MunicipalityData.Name ?? string.Empty,
                    LogoPath = dto.MunicipalityData.LogoPath ?? string.Empty
                };
            }

            // Map OpeningHours / TemporaryClosure / Booking (owned)
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
                        }
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
                        }
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
                        }
                };
            }

            // Entity base
            var entity = new ShoppingCardDetail
            {
                Identifier = Guid.TryParse(dto.Identifier, out var identifier) && identifier != Guid.Empty ? identifier : Guid.NewGuid(),
                OfficialName = dto.OfficialName?.Trim(),
                Address = dto.Address?.Trim(),
                Description = dto.Description?.Trim(),
                ImagePath = dto.ImagePath?.Trim(),
                PoiCategory = dto.PoiCategory?.Trim(),
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Email = dto.Email?.Trim(),
                Telephone = dto.Telephone?.Trim(),
                Website = dto.Website?.Trim(),
                Facebook = dto.Facebook?.Trim(),
                Instagram = dto.Instagram?.Trim(),
                NearestCarPark = nearestCarPark,
                Owner = owner,
                OpeningHours = openingHours,
                TemporaryClosure = temporaryClosure,
                Booking = booking,
                MunicipalityData = municipality
            };

            // Primitive collections
            if (dto.Gallery != null)
            {
                foreach (var g in dto.Gallery)
                {
                    if (!string.IsNullOrWhiteSpace(g))
                        entity.Gallery.Add(g.Trim());
                }
            }

            if (dto.VirtualTours != null)
            {
                foreach (var vt in dto.VirtualTours)
                {
                    if (!string.IsNullOrWhiteSpace(vt))
                        entity.VirtualTours.Add(vt.Trim());
                }
            }

            // AssociatedServices
            if (dto.AssociatedServices != null && dto.AssociatedServices.Any())
            {
                foreach (var a in dto.AssociatedServices)
                {
                    if (a is null) continue;

                    Guid.TryParse(a.Identifier, out var serviceId);
                    if (serviceId == Guid.Empty)
                        serviceId = Guid.NewGuid();

                    entity.AssociatedServices.Add(new AssociatedService
                    {
                        Identifier = serviceId,
                        Name = a.Name ?? string.Empty,
                        ImagePath = a.ImagePath ?? string.Empty
                    });
                }
            }

            // Services (owned list in ShoppingCardDetail)
            if (dto.Services != null && dto.Services.Any())
            {
                foreach (var s in dto.Services)
                {
                    if (s is null) continue;

                    entity.Services.Add(new PointOfSaleService
                    {
                        Name = s.Name?.Trim(),
                        Description = s.Description?.Trim()
                    });
                }
            }

            // SellingTypicalProducts
            if (dto.SellingTypicalProducts != null && dto.SellingTypicalProducts.Any())
            {
                foreach (var tp in dto.SellingTypicalProducts)
                {
                    if (tp is null) continue;

                    entity.SellingTypicalProducts.Add(new TypicalProduct
                    {
                        Identifier = tp.Identifier,
                        Name = tp.Name?.Trim(),
                        Description = tp.Description?.Trim(),
                        Address = tp.Address?.Trim(),
                        CityName = tp.CityName?.Trim(),
                        CreatedAt = tp.CreatedAt,
                        Status = tp.Status,
                        Type = tp.Type,
                        Certification = tp.Certification
                    });
                }
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
                    var fcr = new FeatureCardRelationship<ShoppingCardDetail> { FeatureCard = n, RelatedEntity = entity };
                    entity.Neighbors.Add(fcr);
                    n.ShoppingRelations.Add(fcr);
                }
            }

            return entity;
        }
    }
}