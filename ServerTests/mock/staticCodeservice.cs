

using nam.Server.Models.Services.Infrastructure;

namespace nam.ServerTests.mock
{
    public class StaticCodeService : ICodeService
    {
        public int TimeToLiveMinutes => 15;

        public string GenerateAuthCode()=> "123456";
    }
}
