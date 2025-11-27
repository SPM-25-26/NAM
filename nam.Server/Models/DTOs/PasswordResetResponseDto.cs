
namespace nam.Server.Models.DTOs
{
    public record PasswordResetResponseDto
    {
        public required bool Success { get; init; }
        public required string Message { get; init; }
    }
}
