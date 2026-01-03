using Domain.Entities.MunicipalityEntities;

namespace DataInjection.DTOs
{
    public class FeatureCardDto
    {
        public string? EntityId { get; set; }
        public string? Title { get; set; }
        public MobileCategory? Category { get; set; }
        public string? ImagePath { get; set; }
        public string? ExtraInfo { get; set; }
    }
}