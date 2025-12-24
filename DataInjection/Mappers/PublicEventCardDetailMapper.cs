using DataInjection.Interfaces;
using Domain.DTOs.MunicipalityInjection;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.Mappers
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
                NearestCarPark = nearestCarPark,
                Date = dto.Date,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Organizer = organizer,
                TicketsAndCosts = offers,
                MunicipalityData = municipality,
            };

            // Map Neighbors (FeatureCard)
            List<FeatureCard>? neighbors = null;
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
                var fcr = new FeatureCardRelationship<PublicEventMobileDetail> { FeatureCard = n, RelatedEntity = entity };
                entity.Neighbors.Add(fcr);
                n.PublicEventMobileDetailRelations.Add(fcr);
            }
            return entity;
        }
    }
}
