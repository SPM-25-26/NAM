namespace nam.Server.Models.DTOs.MunicipalityInjection
{
    public class MunicipalityHomeInfoDto
    {
        public string? Name { get; set; }
        public string? LegalName { get; set; }
        public string? Description { get; set; }
        public MunicipalityHomeContactInfoDto Contacts { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? LogoPath { get; set; }
        public ICollection<string>? HomeImages { get; set; }
        public ICollection<FeatureCardDto>? Events { get; set; } = [];
        public ICollection<FeatureCardDto>? ArticlesAndPaths { get; set; } = [];
        public string? PanoramaPath { get; set; }
        public int? PanoramaWidth { get; set; }
        public ICollection<string>? VirtualTourUrls { get; set; } = [];
        public string? NameAndProvince { get; set; }
    }

    public class MunicipalityHomeContactInfoDto
    {
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? Website { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
    }
}
