using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class PublicEventMobileDetail
    {
        [Key]
        public Guid Identifier { get; set; }

        [Embeddable]
        public string? Title { get; set; }

        public string? Address { get; set; }

        [Embeddable]
        public string? Description { get; set; }

        [Embeddable]
        public string? Typology { get; set; }

        public string? PrimaryImage { get; set; }

        public List<string>? Gallery { get; set; }

        public List<string>? VirtualTours { get; set; }

        [Embeddable]
        public string? Audience { get; set; }

        public string? Email { get; set; }

        public string? Telephone { get; set; }

        public string? Website { get; set; }

        public string? Facebook { get; set; }

        public string? Instagram { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Embeddable]
        public virtual List<FeatureCardRelationship<PublicEventMobileDetail>> Neighbors { get; set; } = [];

        [Embeddable]
        public virtual NearestCarPark? NearestCarPark { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Embeddable]
        public virtual Organizer? Organizer { get; set; }

        [Embeddable]
        public virtual List<Offer>? TicketsAndCosts { get; set; }

        [Embeddable]
        public virtual MunicipalityForLocalStorageSetting? MunicipalityData { get; set; }
    }
}
