namespace nam.Server.Models.DTOs.MunicipalityInjection
{
    public class PublicEventMobileDetailDto
    {
        public string? Identifier { get; set; }

        public string? Title { get; set; }

        public string? Address { get; set; }

        public string? Description { get; set; }

        public string? Typology { get; set; }

        public string? PrimaryImage { get; set; }

        public List<string>? Gallery { get; set; }

        public List<string>? VirtualTours { get; set; }

        public string? Audience { get; set; }

        public string? Email { get; set; }

        public string? Telephone { get; set; }

        public string? Website { get; set; }

        public string? Facebook { get; set; }

        public string? Instagram { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public List<FeatureCardDto>? Neighbors { get; set; }

        public NearestCarParkDto? NearestCarPark { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public OrganizerDto? Organizer { get; set; }

        public List<OfferDto>? TicketsAndCosts { get; set; }

        public MunicipalityForLocalStorageSettingDto? MunicipalityData { get; set; }
    }
}
