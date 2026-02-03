using DataInjection.Core.Interfaces;
using DataInjection.SQL.DTOs;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.SQL.Mappers
{
    public class SleepCardDetailMapper : IDtoMapper<SleepCardDetailDto, SleepCardDetail>
    {
        public SleepCardDetail MapToEntity(SleepCardDetailDto dto)
        {
            if (dto is null)
            {
                return new SleepCardDetail
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

            // Map Owner (owned) - stessa logica usata nello Shopping
            Owner? owner = null;
            if (dto.Owner != null && !string.IsNullOrWhiteSpace(dto.Owner.TaxCode))
            {
                owner = new Owner
                {
                    TaxCode = dto.Owner.TaxCode.Trim(),
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
                            Description = null
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
                        }
                };
            }

            // Map Offers
            List<Offer>? offers = null;
            if (dto.Offers != null && dto.Offers.Any())
            {
                offers = dto.Offers
                    .Where(o => o is not null)
                    .Select(o => new Offer
                    {
                        Description = o!.Description,
                        PriceSpecificationCurrencyValue = o.PriceSpecificationCurrencyValue,
                        Currency = o.Currency,
                        ValidityDescription = o.ValidityDescription,
                        ValidityStartDate = o.ValidityStartDate,
                        ValidityEndDate = o.ValidityEndDate,
                        UserTypeName = o.UserTypeName,
                        UserTypeDescription = o.UserTypeDescription,
                        TicketDescription = o.TicketDescription
                    })
                    .ToList();
            }

            // Entity base
            var entity = new SleepCardDetail
            {
                Identifier = Guid.TryParse(dto.Identifier, out var identifier) && identifier != Guid.Empty ? identifier : Guid.NewGuid(),

                OfficialName = dto.OfficialName?.Trim(),
                Description = dto.Description?.Trim(),
                Classification = dto.Classification?.Trim(),
                Typology = dto.Typology?.Trim(),
                PrimaryImage = dto.PrimaryImage?.Trim(),

                Email = dto.Email?.Trim(),
                Telephone = dto.Telephone?.Trim(),
                Website = dto.Website?.Trim(),
                Facebook = dto.Facebook?.Trim(),
                Instagram = dto.Instagram?.Trim(),

                Latitude = dto.Latitude,
                Longitude = dto.Longitude,

                NearestCarPark = nearestCarPark,
                Owner = owner,
                OpeningHours = openingHours,
                TemporaryClosure = temporaryClosure,
                Booking = booking,
                ShortAddress = dto.ShortAddress?.Trim(),
                Offers = offers,
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

            if (dto.Services != null)
            {
                foreach (var s in dto.Services)
                {
                    if (!string.IsNullOrWhiteSpace(s))
                        entity.Services.Add(s.Trim());
                }
            }

            if (dto.RoomTypologies != null)
            {
                foreach (var rt in dto.RoomTypologies)
                {
                    if (!string.IsNullOrWhiteSpace(rt))
                        entity.RoomTypologies.Add(rt.Trim());
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
                    var fcr = new FeatureCardRelationship<SleepCardDetail> { FeatureCard = n, RelatedEntity = entity };
                    entity.Neighbors.Add(fcr);
                    n.SleepRelations.Add(fcr);
                }
            }

            return entity;
        }
    }
}