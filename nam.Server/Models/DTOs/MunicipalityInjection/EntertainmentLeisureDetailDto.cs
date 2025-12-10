namespace nam.Server.Models.DTOs.MunicipalityInjection
{
    public class EntertainmentLeisureDetailDto
    {
        public required string? Identifier { get; set; }

        public required string? OfficialName { get; set; }

        public required string? Address { get; set; }

        public string? Description { get; set; }

        public required string? Category { get; set; }

        public required string? PrimaryImage { get; set; }

        public List<string>? Gallery { get; set; }

        public required List<string>? VirtualTours { get; set; }

        public required double Latitude { get; set; }

        public required double Longitude { get; set; }

        public List<FeatureCardDto>? Neighbors { get; set; }

        public NearestCarParkDto? NearestCarPark { get; set; }

        public required List<AssociatedServiceDto>? AssociatedServices { get; set; }

        public required MunicipalityForLocalStorageSettingDto MunicipalityData { get; set; }
    }
}
