namespace nam.Server.DTOs
{
    public record PasswordResetResponseDto
    {
        public required bool Success { get; init; }
        public required string Message { get; init; }
    }
}
