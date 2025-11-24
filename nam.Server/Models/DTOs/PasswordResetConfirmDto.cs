using System.ComponentModel.DataAnnotations;

namespace nam.Server.Models.DTOs
{
    public record PasswordResetConfirmDto
    {
        [Required]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Auth code must be exactly 6 digits.")]
        public required string AuthCode { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public required string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public required string ConfirmPassword { get; set; }
    }
}
