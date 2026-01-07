using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Questionaire
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<string> Interest { get; set; } = [];
        public List<string> TravelStyle { get; set; } = [];
        public string AgeRange { get; set; } = string.Empty;
        public string TravelRange { get; set; } = string.Empty;
        public List<string> TravelCompanions { get; set; } = [];
        public string DiscoveryMode { get; set; } = string.Empty;
    }
}
