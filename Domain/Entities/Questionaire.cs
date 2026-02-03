using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Questionaire
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Embeddable]
        public List<string> Interest { get; set; } = [];

        [Embeddable]
        public List<string> TravelStyle { get; set; } = [];

        [Embeddable]
        public string AgeRange { get; set; } = string.Empty;

        [Embeddable]
        public string TravelRange { get; set; } = string.Empty;

        [Embeddable]
        public List<string> TravelCompanions { get; set; } = [];

        [Embeddable]
        public string DiscoveryMode { get; set; } = string.Empty;

        public float[] Vector { get; set; } = [];
    }
}
