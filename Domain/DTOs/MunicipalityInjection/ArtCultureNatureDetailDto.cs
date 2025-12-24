namespace Domain.DTOs.MunicipalityInjection
{
    public class ArtCultureNatureDetailDto
    {
        public string? Identifier { get; set; }
        public string? OfficialName { get; set; }
        public string? PrimaryImagePath { get; set; }
        public string? FullAddress { get; set; }
        public string? Type { get; set; }

        public List<CulturalSiteServiceDto>? Services { get; set; }
        public List<CulturalProjectDto>? CulturalProjects { get; set; }
        public List<CatalogueDto>? Catalogues { get; set; }
        public List<CreativeWorkMobileDto>? CreativeWorks { get; set; }

        public List<string>? Gallery { get; set; }
        public List<string>? VirtualTours { get; set; }

        public string? SubjectDiscipline { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? Website { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<FeatureCardDto>? Neighbors { get; set; }
        public NearestCarParkDto? NearestCarPark { get; set; }
        public SiteCardDto? Site { get; set; }

        public MunicipalityForLocalStorageSettingDto MunicipalityData { get; set; } = null!;

        public List<AssociatedServiceDto>? AssociatedServices { get; set; }
    }
}
