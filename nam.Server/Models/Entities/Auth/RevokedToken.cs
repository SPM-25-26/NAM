namespace nam.Server.Models.Entities.Auth
{
    public class RevokedToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Jti { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }
    }
}
