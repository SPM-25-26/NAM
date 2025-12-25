namespace DataInjection.DTOs
{
    public class PublicEventCardDto
    {
        public string? EntityId { get; set; }

        public string? EntityName { get; set; }

        public string? ImagePath { get; set; }

        public string? BadgeText { get; set; }

        public string? Address { get; set; }

        public MunicipalityForLocalStorageSettingDto? MunicipalityData { get; set; }

        public string? Date { get; set; }

    }
}
