namespace nam.Server.Models.DTOs.MunicipalityInjection
{
    public class OrganizationCardDto
    {
        public string? EntityId { get; set; }

        public string? EntityName { get; set; }

        public string? ImagePath { get; set; }

        public string? BadgeText { get; set; }

        public string? Address { get; set; }
    }

    public class OrganizationMobileDetailDto
    {
        public string? TaxCode { get; set; }

        public string? LegalName { get; set; }

        public string? PrimaryImagePath { get; set; }

        public string? Type { get; set; }

        public string? Address { get; set; }

        public string? Description { get; set; }

        public string? MainFunction { get; set; }

        public DateTime? FoundationDate { get; set; }

        public string? LegalStatus { get; set; }

        public List<string>? Gallery { get; set; }

        public string? Email { get; set; }

        public string? Telephone { get; set; }

        public string? Website { get; set; }

        public string? Instagram { get; set; }

        public string? Facebook { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public List<FeatureCardDto>? Neighbors { get; set; }

        public NearestCarParkDto? NearestCarPark { get; set; }

        public List<OwnedPoiDto>? OwnedPoi { get; set; }

        public List<OfferDto>? Offers { get; set; }

        public List<PublicEventCardDto>? Events { get; set; }

        public MunicipalityForLocalStorageSettingDto? MunicipalityData { get; set; }
    }

    public class OwnedPoiDto
    {
        public string? Identifier { get; set; }

        public string? OfficialName { get; set; }

        public string? ImagePath { get; set; }

        public string? Category { get; set; }
    }
}
