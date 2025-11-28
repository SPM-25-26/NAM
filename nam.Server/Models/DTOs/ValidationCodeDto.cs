using System.ComponentModel.DataAnnotations;

namespace nam.Server.Models.DTOs
{
    public record ValidationCodeDto
    {
        [Required]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Auth code must be exactly 6 digits.")]
        public required string AuthCode { get; set; }

    }
}
