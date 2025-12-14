using System.ComponentModel.DataAnnotations;

namespace nam.Server.Models.Entities.Auth
{
    public class PasswordResetCode
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public required string UserId { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Auth code must be 6 digits.")]
        public required string AuthCode { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public required DateTime ExpiresAt { get; set; }
        public override string ToString()
        {
            return $"PasswordResetCode [Id: {Id}, UserId: {UserId}, AuthCode: {AuthCode}, CreatedAt: {CreatedAt:yyyy-MM-dd HH:mm:ss}, ExpiresAt: {ExpiresAt:yyyy-MM-dd HH:mm:ss}]";
        }
    }
}


