using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nam.Server.Models.Entities.MunicipalityEntities
{
    // Card entity for Eat & Drink POI (lightweight card shown in lists)
    public class EatAndDrinkCard
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // External identifier (kept as string because DTO exposes entityId as string)
        [MaxLength(100)]
        public string? EntityId { get; set; }

        [Required]
        [MaxLength(500)]
        public required string EntityName { get; set; }

        [Required]
        [MaxLength(1000)]
        public required string ImagePath { get; set; }

        [Required]
        [MaxLength(100)]
        public required string BadgeText { get; set; }

        [MaxLength(1000)]
        public string? Address { get; set; }

        // Optional FK to detailed entity
        public Guid? DetailIdentifier { get; set; }

        [ForeignKey(nameof(DetailIdentifier))]
        public EatAndDrinkDetail? Detail { get; set; }
    }

    // Detailed entity for Eat & Drink POI
    public class EatAndDrinkDetail
    {
        [Key]
        public Guid Identifier { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(1000)]
        public required string PrimaryImagePath { get; set; }

        [Required]
        [MaxLength(500)]
        public required string OfficialName { get; set; }

        [Required]
        [MaxLength(1000)]
        public required string FullAddress { get; set; }

        [Required]
        [MaxLength(4000)]
        public required string Description { get; set; }

        [MaxLength(255)]
        public string? Email { get; set; }

        [MaxLength(50)]
        public string? Telephone { get; set; }

        [MaxLength(500)]
        public string? Facebook { get; set; }

        [MaxLength(500)]
        public string? Instagram { get; set; }

        [MaxLength(500)]
        public string? Website { get; set; }

        [MaxLength(100)]
        public string? Type { get; set; }

        // Primitive collections for gallery and virtual tours
        public List<string> Gallery { get; set; } = new();

        public List<string> VirtualTours { get; set; } = new();

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        // Services: 1..* OntoremaService (mapped from OntoremaServiceDto array)
        public ICollection<OntoremaService>? Services { get; set; }

        // Neighbors -> FeatureCard entity exists in the project (referenced in migrations)
        public ICollection<FeatureCard>? Neighbors { get; set; }

        // Nearest car park (entity exists in project)
        public Guid? NearestCarParkId { get; set; }

        [ForeignKey(nameof(NearestCarParkId))]
        public NearestCarPark? NearestCarPark { get; set; }

        // OpeningHours / TemporaryClosure / Booking mapped to created entities below
        public OpeningHoursSpecification? OpeningHours { get; set; }

        public TemporaryClosure? TemporaryClosure { get; set; }

        public Booking? Booking { get; set; }

        // Dietary needs - simple string list
        public List<string>? DietaryNeeds { get; set; }

        // TypicalProducts (TypicalProductMobileDto) not created because referenced class not present.
        public ICollection<TypicalProductMobile>? TypicalProducts { get; set; }

        // Owner (OwnerDto) not created because referenced class not present.
        public Owner? Owner { get; set; }

        // AssociatedServices -> AssociatedService entity exists in the project
        public ICollection<AssociatedService>? AssociatedServices { get; set; }

        // Municipality data -> MunicipalityForLocalStorageSetting entity exists
        public Guid? MunicipalityDataId { get; set; }

        [ForeignKey(nameof(MunicipalityDataId))]
        public MunicipalityForLocalStorageSetting? MunicipalityData { get; set; }
    }

    // --- New DB entities created from the requested DTOs ---

    /// <summary>
    /// Represents a service as in OntoremaServiceDto (name required, description optional).
    /// Linked to EatAndDrinkDetail.
    /// </summary>
    public class OntoremaService
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(255)]
        public string? Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        // FK back to EatAndDrinkDetail
        public Guid? EatAndDrinkDetailIdentifier { get; set; }

        [ForeignKey(nameof(EatAndDrinkDetailIdentifier))]
        public EatAndDrinkDetail? EatAndDrinkDetail { get; set; }
    }

    /// <summary>
    /// Time interval entity used by OpeningHoursSpecification, TemporaryClosure and Booking.
    /// Kept simple: start/end DateTime.
    /// </summary>
    public class TimeInterval
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Start and End are nullable to allow flexible usage (some DTOs may only partially specify)
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }

    /// <summary>
    /// Opening hours spec corresponding to OpeningHoursSpecificationDto.
    /// AdmissionType and TimeInterval are required in the DTO: enforce AdmissionType as required string and TimeInterval FK as required.
    /// </summary>
    public class OpeningHoursSpecification
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // times of day (nullable)
        public TimeSpan? Opens { get; set; }
        public TimeSpan? Closes { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        // AdmissionType placeholder as string (AdmissionTypeDto not created)
        [Required]
        [MaxLength(100)]
        public required string AdmissionType { get; set; }

        // Required TimeInterval
        [Required]
        public Guid TimeIntervalId { get; set; }

        [ForeignKey(nameof(TimeIntervalId))]
        public TimeInterval? TimeInterval { get; set; }

        // Optional day of week
        public DayOfWeek? Day { get; set; }

        // FK to parent detail
        public Guid? EatAndDrinkDetailIdentifier { get; set; }

        [ForeignKey(nameof(EatAndDrinkDetailIdentifier))]
        public EatAndDrinkDetail? EatAndDrinkDetail { get; set; }
    }

    /// <summary>
    /// Temporary closure corresponding to TemporaryClosureDto.
    /// </summary>
    public class TemporaryClosure
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Required in DTO but nullable content allowed
        [MaxLength(1000)]
        public string? ReasonForClosure { get; set; }

        public TimeSpan? Opens { get; set; }
        public TimeSpan? Closes { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        // Required TimeInterval
        [Required]
        public Guid TimeIntervalId { get; set; }

        [ForeignKey(nameof(TimeIntervalId))]
        public TimeInterval? TimeInterval { get; set; }

        public DayOfWeek? Day { get; set; }

        // FK to parent detail
        public Guid? EatAndDrinkDetailIdentifier { get; set; }

        [ForeignKey(nameof(EatAndDrinkDetailIdentifier))]
        public EatAndDrinkDetail? EatAndDrinkDetail { get; set; }
    }

    /// <summary>
    /// Booking entity corresponding to BookingDto.
    /// </summary>
    public class Booking
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Optional TimeInterval
        public Guid? TimeIntervalId { get; set; }

        [ForeignKey(nameof(TimeIntervalId))]
        public TimeInterval? TimeInterval { get; set; }

        // Name is required in DTO; use string (BookingType not created)
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        // FK to parent detail
        public Guid? EatAndDrinkDetailIdentifier { get; set; }

        [ForeignKey(nameof(EatAndDrinkDetailIdentifier))]
        public EatAndDrinkDetail? EatAndDrinkDetail { get; set; }
    }

    public class TypicalProductMobile
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // 'name': Required
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        // 'description': Required
        // Using MaxLength(2000) for a substantial paragraph. 
        // Remove MaxLength if you want nvarchar(max).
        [Required]
        [MaxLength(2000)]
        public required string Description { get; set; }

        public Guid? EatAndDrinkDetailIdentifier { get; set; }

        [ForeignKey(nameof(EatAndDrinkDetailIdentifier))]
        public EatAndDrinkDetail? EatAndDrinkDetail { get; set; }
    }

    public class Owner
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // 'taxCode': Required. 
        // This is often a business unique identifier (VAT/Fiscal Code).
        [Required]
        [MaxLength(100)]
        public required string TaxCode { get; set; }

        // 'legalName': Required
        [Required]
        [MaxLength(500)]
        public required string LegalName { get; set; }

        // 'website': Nullable
        [MaxLength(1000)]
        [Url]
        public string? Website { get; set; }

        public Guid? EatAndDrinkDetailIdentifier { get; set; }

        [ForeignKey(nameof(EatAndDrinkDetailIdentifier))]
        public EatAndDrinkDetail? EatAndDrinkDetail { get; set; }
    }
}