using System.ComponentModel.DataAnnotations;

namespace nam.Server.Models.Entities
{
    public class Category
    {
        [Key]
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string ImagePath { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Label { get; set; } = string.Empty;
    }
}