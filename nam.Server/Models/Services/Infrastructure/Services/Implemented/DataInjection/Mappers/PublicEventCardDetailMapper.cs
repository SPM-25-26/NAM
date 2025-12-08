using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Mappers
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

            // Identifier
            Guid.TryParse(dto.Identifier, out Guid identifier);
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
            Guid? organizerId = null;
            if (dto.Organizer != null)
            {
                organizer = new Organizer
                {
                    Id = Guid.NewGuid(),
                    TaxCode = dto.Organizer.TaxCode?.Trim(),
                    LegalName = dto.Organizer.LegalName?.Trim(),
                    Website = dto.Organizer.Website?.Trim()
                };
                organizerId = organizer.Id;
            }

            // Map Municipality data
            MunicipalityForLocalStorageSetting? municipality = null;
            Guid? municipalityId = null;
            if (dto.MunicipalityData != null)
            {
                municipality = new MunicipalityForLocalStorageSetting
                {
                    Id = Guid.NewGuid(),
                    Name = dto.MunicipalityData.Name ?? string.Empty,
                    LogoPath = dto.MunicipalityData.LogoPath ?? string.Empty
                };
                municipalityId = municipality.Id;
            }

            // Map Neighbors (FeatureCard)
            List<FeatureCard>? neighbors = null;
            if (dto.Neighbors != null && dto.Neighbors.Any())
            {
                neighbors = dto.Neighbors.Select(n => new FeatureCard
                {
                    Id = Guid.NewGuid(),
                    EntityId = n.EntityId?.Trim(),
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
                    PublicEventMobileDetailIdentifier = identifier
                }).ToList();
            }

            var entity = new PublicEventMobileDetail
            {
                Identifier = identifier,
                Title = dto.Title?.Trim(),
                Address = dto.Address?.Trim(),
                Description = dto.Description?.Trim(),
                Typology = dto.Typology?.Trim(),
                PrimaryImage = dto.PrimaryImage?.Trim(),
                Gallery = dto.Gallery != null ? new List<string>(dto.Gallery) : new List<string>(),
                VirtualTours = dto.VirtualTours != null ? new List<string>(dto.VirtualTours) : new List<string>(),
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
