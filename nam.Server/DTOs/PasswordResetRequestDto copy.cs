using System.ComponentModel.DataAnnotations;

namespace nam.Server.DTOs
{
    public record PasswordResetRequestDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
