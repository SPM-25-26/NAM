using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nam.Server.Models.Entities.MunicipalityEntities
{
    public class PublicEventMobileDetail
    {
        [Key]
        public Guid Identifier { get; set; }

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

        public virtual List<FeatureCard>? Neighbors { get; set; }

        public Guid? NearestCarParkId { get; set; }

        [ForeignKey(nameof(NearestCarParkId))]
        public virtual NearestCarPark? NearestCarPark { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Guid? OrganizerId { get; set; }

        [ForeignKey(nameof(OrganizerId))]
        public virtual Organizer? Organizer { get; set; }

        public virtual List<Offer>? TicketsAndCosts { get; set; }

        public Guid? MunicipalityDataId { get; set; }
        [ForeignKey(nameof(MunicipalityDataId))]
        public virtual MunicipalityForLocalStorageSetting? MunicipalityData { get; set; }
    }
}
