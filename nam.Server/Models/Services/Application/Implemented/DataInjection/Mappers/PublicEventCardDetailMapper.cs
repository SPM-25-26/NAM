using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Application.Interfaces.DataInjection;

namespace nam.Server.Models.Services.Application.Implemented.DataInjection.Mappers
{
    public class PublicEventCardDetailMapper : IDtoMapper<PublicEventMobileDetailDto, PublicEventMobileDetail>
    {
        public PublicEventMobileDetail MapToEntity(PublicEventMobileDetailDto dto)
        {
            if (dto is null)
            {
                return new PublicEventMobileDetail
                {
                    Identifier = Guid.NewGuid()
                };
            }

            // Map NearestCarPark
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

            // Map Organizer
            Organizer? organizer = null;
            if (dto.Organizer != null && dto.Organizer.TaxCode != null)
            {
                organizer = new Organizer
                {
                    TaxCode = dto.Organizer.TaxCode?.Trim(),
                    LegalName = dto.Organizer.LegalName?.Trim(),
                    Website = dto.Organizer.Website?.Trim()
                };
            }

            // Map Municipality data
            MunicipalityForLocalStorageSetting? municipality = null;
            if (dto.MunicipalityData != null)
            {
                municipality = new MunicipalityForLocalStorageSetting
                {
                    Name = dto.MunicipalityData.Name ?? string.Empty,
                    LogoPath = dto.MunicipalityData.LogoPath ?? string.Empty
                };
            }

            // Map Neighbors (FeatureCard)
            List<FeatureCard>? neighbors = null;
            if (dto.Neighbors != null && dto.Neighbors.Any())
            {
                neighbors = dto.Neighbors.Select(n => new FeatureCard
                {
                    EntityId = Guid.TryParse(n.EntityId, out var guid) ? guid : Guid.NewGuid(),
                    Title = n.Title?.Trim(),
                    Category = n.Category ?? default,
                    ImagePath = n.ImagePath?.Trim(),
                    ExtraInfo = n.ExtraInfo
                }).ToList();
            }

            // Map Tickets / Offers
            List<Offer>? offers = null;
            if (dto.TicketsAndCosts != null && dto.TicketsAndCosts.Any())
            {
                offers = dto.TicketsAndCosts.Select(o => new Offer
                {
                    // Id left default; EF will generate if needed
                    Description = o.Description,
                    PriceSpecificationCurrencyValue = o.PriceSpecificationCurrencyValue,
                    Currency = o.Currency,
                    ValidityDescription = o.ValidityDescription,
                    ValidityStartDate = o.ValidityStartDate,
                    ValidityEndDate = o.ValidityEndDate,
                    UserTypeName = o.UserTypeName,
                    UserTypeDescription = o.UserTypeDescription,
                    TicketDescription = o.TicketDescription,
                }).ToList();
            }

            var entity = new PublicEventMobileDetail
            {
                Identifier = Guid.TryParse(dto.Identifier, out Guid identifier) ? identifier : Guid.NewGuid(),
                Title = dto.Title?.Trim(),
                Address = dto.Address?.Trim(),
                Description = dto.Description?.Trim(),
                Typology = dto.Typology?.Trim(),
                PrimaryImage = dto.PrimaryImage?.Trim(),
                Gallery = dto.Gallery != null ? dto.Gallery : [],
                VirtualTours = dto.VirtualTours != null ? dto.VirtualTours : [],
                Audience = dto.Audience?.Trim(),
                Email = dto.Email?.Trim(),
                Telephone = dto.Telephone?.Trim(),
                Website = dto.Website?.Trim(),
                Facebook = dto.Facebook?.Trim(),
                Instagram = dto.Instagram?.Trim(),
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Neighbors = neighbors,
                NearestCarPark = nearestCarPark,
                Date = dto.Date,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Organizer = organizer,
                TicketsAndCosts = offers,
                MunicipalityData = municipality,
            };

            return entity;
        }
    }
}
