namespace nam.Server.Models.DTOs
{
    public class VerifyEmailRequestDto
    {
        public string Email { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}
