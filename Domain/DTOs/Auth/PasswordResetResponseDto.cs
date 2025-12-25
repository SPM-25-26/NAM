namespace Domain.DTOs.Auth
{
    public record PasswordResetResponseDto
    {
        public required bool Success { get; init; }
        public required string Message { get; init; }
    }
}
