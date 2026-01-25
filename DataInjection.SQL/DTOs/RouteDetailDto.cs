namespace DataInjection.SQL.DTOs
{
    public class RouteDetailDto
    {
        public string? ImagePath { get; set; }

        public string? Number { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? PathTheme { get; set; }

        public string? TravellingMethod { get; set; }

        public string? ShortName { get; set; }

        public string? OrganizationWebsite { get; set; }

        public string? OrganizationEmail { get; set; }

        public string? OrganizationFacebook { get; set; }

        public string? OrganizationInstagram { get; set; }

        public string? OrganizationTelephone { get; set; }

        public string? Website { get; set; }

        public string? SecurityLevel { get; set; }

        public int NumberOfStages { get; set; }

        public int QuantifiedPathwayPaving { get; set; }

        public string? Duration { get; set; }

        public string? RouteLength { get; set; }

        public List<string?>? Gallery { get; set; }

        public List<string?>? VirtualTours { get; set; }

        public List<StageMobileDto?>? Stages { get; set; }

        public List<FeatureCardDto?>? StagesPoi { get; set; }

        public PointDto? StartingPoint { get; set; }

        public List<string?>? BestWhen { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public MunicipalityForLocalStorageSettingDto? MunicipalityData { get; set; }
    }
}
