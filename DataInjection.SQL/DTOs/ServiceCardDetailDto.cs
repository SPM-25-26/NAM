using System;

namespace DataInjection.SQL.DTOs
{
    public class ServiceCardDetailDto
    {
        public string? Identifier { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }

        public int? SpacesForDisabled { get; set; }
        public int? PayingParkingSpaces { get; set; }
        public int? AvailableParkingSpaces { get; set; }
        public int? PostiAutoVenduti { get; set; }
        public int? TotalNumberOfCarParkSpaces { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string? Typology { get; set; }
        public string? PrimaryImage { get; set; }

        public List<string>? Gallery { get; set; }

        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? Website { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }

        public string? Price { get; set; }
        public string? ReservationUrl { get; set; }

        public List<FeatureCardDto?>? Neighbors { get; set; }
        public NearestCarParkDto? NearestCarPark { get; set; }
        public OpeningHoursSpecificationDto? OpeningHours { get; set; }
        public TemporaryClosureDto? TemporaryClosure { get; set; }
        public BookingDto? Booking { get; set; }

        public List<ServiceLocationDto?>? Locations { get; set; }

        public MunicipalityForLocalStorageSettingDto? MunicipalityData { get; set; } = null!;

        public List<AssociatedServiceDto?>? AssociatedServices { get; set; }
    }
}