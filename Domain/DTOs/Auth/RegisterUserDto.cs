using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Auth
{
    public record RegisterUserDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public required string Password { get; set; }

        [Compare("Password")]
        public required string ConfirmPassword { get; set; }
    }
}
