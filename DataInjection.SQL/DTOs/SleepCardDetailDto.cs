using Domain.Entities.MunicipalityEntities;

namespace DataInjection.SQL.DTOs
{
    public class SleepCardDetailDto
    {
        public string Identifier { get; set; }

        public string? OfficialName { get; set; }
        public string? Description { get; set; }

        public string? Classification { get; set; }
        public string? Typology { get; set; }

        public string? PrimaryImage { get; set; }

        public List<string?>? Gallery { get; set; }

        public List<string?>? VirtualTours { get; set; }

        public List<string?>? Services { get; set; }

        public List<string?>? RoomTypologies { get; set; }

        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? Website { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<FeatureCardDto?>? Neighbors { get; set; }

        public NearestCarParkDto? NearestCarPark { get; set; }

        public OpeningHoursSpecificationDto? OpeningHours { get; set; }

        public TemporaryClosureDto? TemporaryClosure { get; set; }

        public OwnerDto? Owner { get; set; }

        public BookingDto? Booking { get; set; }

        public string? ShortAddress { get; set; }

        public List<OfferDto?>? Offers { get; set; }

        public List<AssociatedServiceDto?>? AssociatedServices { get; set; }

        public MunicipalityForLocalStorageSettingDto MunicipalityData { get; set; } = null!;
    }
}