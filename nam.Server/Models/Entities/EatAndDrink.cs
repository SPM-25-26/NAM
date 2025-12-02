using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nam.Server.Models.Entities
{
    public class EatAndDrink
    {
        [Key]
        [Required]
        public Guid EntityId { get; set; }

        [Required]
        [MaxLength(500)]
        public string EntityName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string ImagePath { get; set; } = string.Empty;

        [MaxLength(100)]
        public string BadgeText { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Address { get; set; } = string.Empty;

        // Navigation to the detailed entity (one-to-one)
        public EatAndDrinkDetail? Detail { get; set; }
    }

    public class EatAndDrinkDetail
    {
        [Key]
        [Required]
        public Guid Identifier { get; set; }

        [MaxLength(1000)]
        public string PrimaryImagePath { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string OfficialName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string Description { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Telephone { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Facebook { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Instagram { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Website { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Type { get; set; } = string.Empty;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Child collections
        public ICollection<EatService> Services { get; set; } = new List<EatService>();
        public ICollection<EatNeighbor> Neighbors { get; set; } = new List<EatNeighbor>();
        public ICollection<EatGalleryImage> Gallery { get; set; } = new List<EatGalleryImage>();
        public ICollection<EatVirtualTour> VirtualTours { get; set; } = new List<EatVirtualTour>();
        public ICollection<TypicalProduct> TypicalProducts { get; set; } = new List<TypicalProduct>();
        public ICollection<EatAssociatedService> AssociatedServices { get; set; } = new List<EatAssociatedService>();
        public ICollection<Menu> Menus { get; set; } = new List<Menu>();
        public ICollection<string> DietaryNeeds { get; set; } = new List<string>();

        // One-to-one / complex objects
        public NearestCarPark? NearestCarPark { get; set; }
        public OpeningHours? OpeningHours { get; set; }
        public TemporaryClosure? TemporaryClosure { get; set; }
        public Booking? Booking { get; set; }
        public EatOwner? Owner { get; set; }
        public EatMunicipalityData? MunicipalityData { get; set; }
    }

    public class EatService
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public Guid EatAndDrinkDetailId { get; set; }

        [ForeignKey(nameof(EatAndDrinkDetailId))]
        public EatAndDrinkDetail EatAndDrinkDetail { get; set; } = null!;
    }

    public class EatNeighbor
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string EntityId { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string ImagePath { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string ExtraInfo { get; set; } = string.Empty;

        [Required]
        public Guid EatAndDrinkDetailId { get; set; }

        [ForeignKey(nameof(EatAndDrinkDetailId))]
        public EatAndDrinkDetail EatAndDrinkDetail { get; set; } = null!;
    }

    public class EatGalleryImage
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(1000)]
        public string ImagePath { get; set; } = string.Empty;

        [Required]
        public Guid EatAndDrinkDetailId { get; set; }

        [ForeignKey(nameof(EatAndDrinkDetailId))]
        public EatAndDrinkDetail EatAndDrinkDetail { get; set; } = null!;
    }

    public class EatVirtualTour
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(1000)]
        public string TourUrl { get; set; } = string.Empty;

        [Required]
        public Guid EatAndDrinkDetailId { get; set; }

        [ForeignKey(nameof(EatAndDrinkDetailId))]
        public EatAndDrinkDetail EatAndDrinkDetail { get; set; } = null!;
    }

    public class OpeningHours
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime? Opens { get; set; }
        public DateTime? Closes { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        public AdmissionType? AdmissionType { get; set; }

        public TimeInterval? TimeInterval { get; set; }

        [MaxLength(50)]
        public string Day { get; set; } = string.Empty;

        [Required]
        public Guid EatAndDrinkDetailId { get; set; }

        [ForeignKey(nameof(EatAndDrinkDetailId))]
        public EatAndDrinkDetail EatAndDrinkDetail { get; set; } = null!;
    }

    public class TemporaryClosure
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(2000)]
        public string ReasonForClosure { get; set; } = string.Empty;

        public DateTime? Opens { get; set; }
        public DateTime? Closes { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        public TimeInterval? TimeInterval { get; set; }

        [MaxLength(50)]
        public string Day { get; set; } = string.Empty;

        [Required]
        public Guid EatAndDrinkDetailId { get; set; }

        [ForeignKey(nameof(EatAndDrinkDetailId))]
        public EatAndDrinkDetail EatAndDrinkDetail { get; set; } = null!;
    }

    public class Booking
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public TimeInterval? TimeInterval { get; set; }

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public Guid EatAndDrinkDetailId { get; set; }

        [ForeignKey(nameof(EatAndDrinkDetailId))]
        public EatAndDrinkDetail EatAndDrinkDetail { get; set; } = null!;
    }

    public class AdmissionType
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;
    }

    public class TimeInterval
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime? Date { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class TypicalProduct
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public Guid EatAndDrinkDetailId { get; set; }

        [ForeignKey(nameof(EatAndDrinkDetailId))]
        public EatAndDrinkDetail EatAndDrinkDetail { get; set; } = null!;
    }

    public class EatOwner
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(100)]
        public string TaxCode { get; set; } = string.Empty;

        [MaxLength(500)]
        public string LegalName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Website { get; set; } = string.Empty;

        [Required]
        public Guid EatAndDrinkDetailId { get; set; }

        [ForeignKey(nameof(EatAndDrinkDetailId))]
        public EatAndDrinkDetail EatAndDrinkDetail { get; set; } = null!;
    }

    public class EatAssociatedService
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(255)]
        public string Identifier { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string ImagePath { get; set; } = string.Empty;

        [Required]
        public Guid EatAndDrinkDetailId { get; set; }

        [ForeignKey(nameof(EatAndDrinkDetailId))]
        public EatAndDrinkDetail EatAndDrinkDetail { get; set; } = null!;
    }

    public class EatMunicipalityData
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string LogoPath { get; set; } = string.Empty;

        [Required]
        public Guid EatAndDrinkDetailId { get; set; }

        [ForeignKey(nameof(EatAndDrinkDetailId))]
        public EatAndDrinkDetail EatAndDrinkDetail { get; set; } = null!;
    }

    public class Menu
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(500)]
        public string RestaurantName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Type { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        public ICollection<MenuSection> Sections { get; set; } = new List<MenuSection>();

        [Required]
        public Guid EatAndDrinkDetailId { get; set; }

        [ForeignKey(nameof(EatAndDrinkDetailId))]
        public EatAndDrinkDetail EatAndDrinkDetail { get; set; } = null!;
    }

    public class MenuSection
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public ICollection<MenuItem> Items { get; set; } = new List<MenuItem>();

        [Required]
        public Guid MenuId { get; set; }

        [ForeignKey(nameof(MenuId))]
        public Menu Menu { get; set; } = null!;
    }

    public class MenuItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(500)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string ImagePath { get; set; } = string.Empty;

        public Price? Price { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public Guid MenuSectionId { get; set; }

        [ForeignKey(nameof(MenuSectionId))]
        public MenuSection MenuSection { get; set; } = null!;
    }

    public class Price
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [MaxLength(10)]
        public string Currency { get; set; } = "EUR";
    }
}
