using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Application.Interfaces.DataInjection;

namespace nam.Server.Models.Services.Application.Implemented.DataInjection.Mappers
{
    public class OrganizationMobileDetailMapper : IDtoMapper<OrganizationMobileDetailDto, OrganizationMobileDetail>
    {
        public OrganizationMobileDetail MapToEntity(OrganizationMobileDetailDto dto)
        {
            return new OrganizationMobileDetail
            {
                TaxCode = dto.TaxCode,
                LegalName = dto.LegalName,
                PrimaryImagePath = dto.PrimaryImagePath,
                Type = dto.Type,
                Address = dto.Address,
                Description = dto.Description,
                MainFunction = dto.MainFunction,
                FoundationDate = dto.FoundationDate,
                LegalStatus = dto.LegalStatus,
                Gallery = dto.Gallery,
                Email = dto.Email,
                Telephone = dto.Telephone,
                Website = dto.Website,
                Instagram = dto.Instagram,
                Facebook = dto.Facebook,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Neighbors = dto.Neighbors?.Select(n => new FeatureCard
                {
                    EntityId = n.EntityId,
                    Title = n.Title,
                    Category = n.Category ?? default,
                    ImagePath = n.ImagePath,
                    ExtraInfo = n.ExtraInfo
                }).ToList(),
                NearestCarPark = dto.NearestCarPark != null ? new NearestCarPark
                {
                    Latitude = dto.NearestCarPark.Latitude,
                    Longitude = dto.NearestCarPark.Longitude,
                    Address = dto.NearestCarPark.Address,
                    Distance = dto.NearestCarPark.Distance
                } : null,
                OwnedPoi = dto.OwnedPoi?
                .Where(p => p != null)
                .Select(p => new OwnedPoi
                {
                    Identifier = p.Identifier,
                    OfficialName = p.OfficialName,
                    ImagePath = p.ImagePath,
                    Category = p.Category
                }).ToList(),
                Offers = dto.Offers?.Select(o => new Offer
                {
                    Description = o.Description,
                    PriceSpecificationCurrencyValue = o.PriceSpecificationCurrencyValue,
                    Currency = o.Currency,
                    ValidityDescription = o.ValidityDescription,
                    ValidityStartDate = o.ValidityStartDate,
                    ValidityEndDate = o.ValidityEndDate,
                    UserTypeName = o.UserTypeName,
                    UserTypeDescription = o.UserTypeDescription,
                    TicketDescription = o.TicketDescription
                }).ToList(),
                Events = dto.Events?
                .Where(p => p != null)
                .Select(e => new PublicEventCard
                {
                    EntityId = Guid.TryParse(e.EntityId, out var guid) ? guid : Guid.NewGuid(),
                    EntityName = e.EntityName,
                    ImagePath = e.ImagePath,
                    BadgeText = e.BadgeText,
                    Address = e.Address,
                    Date = e.Date,
                    MunicipalityData = e.MunicipalityData != null ? new MunicipalityForLocalStorageSetting
                    {
                        Name = e.MunicipalityData.Name,
                        LogoPath = e.MunicipalityData.LogoPath
                    } : null
                }).ToList(),
                MunicipalityData = dto.MunicipalityData != null ? new MunicipalityForLocalStorageSetting
                {
                    Name = dto.MunicipalityData.Name,
                    LogoPath = dto.MunicipalityData.LogoPath
                } : null
            };
        }
    }
}
