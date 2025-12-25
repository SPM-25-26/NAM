using nam.Server.Services.Interfaces.Auth;

namespace nam.Server.Services.Implemented.Auth
{
    public class RandomCodeService : ICodeService
    {
        public int TimeToLiveMinutes => 15;

        public string GenerateAuthCode() => new Random().Next(100000, 999999).ToString();

    }
}