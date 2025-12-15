using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace nam.Server.Models.Entities.MunicipalityEntities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MobileCategory
    {
        Sleep,
        EatAndDrink,
        Events,
        ArtCulture,
        Nature,
        TypicalProducts,
        Routes,
        Services,
        EntertainmentLeisure,
        Organizations,
        Articles,
        Shopping
    }

    public class MobileCategoryDetail
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public MobileCategory Category { get; set; }

        [MaxLength(500)]
        public string? ImagePath { get; set; }

        [MaxLength(100)]
        public string? Label { get; set; }
    }
}