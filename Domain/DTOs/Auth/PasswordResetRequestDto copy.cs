using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Auth
{
    public record PasswordResetRequestDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
