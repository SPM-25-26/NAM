using System.ComponentModel.DataAnnotations;

namespace nam.Server.Models.DTOs
{
    public record PasswordResetRequestDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
