using nam.ServerTests.Integration.Shared;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace nam.ServerTests.NamServer.Endpoints.MunicipalityEntities
{
    [TestFixture]
    public class MunicipalityEntityEndpointsSmokeTests
    {
        private NamTestFactory? _factory;
        private HttpClient? _client;

        [OneTimeSetUp]
        public void Setup()
        {
            _factory = new NamTestFactory();
            _client = _factory.CreateClient();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [TestCase("/api/art-culture/card-list?municipality=TestTown&language=it")]
        [TestCase("/api/article/card-list?municipality=TestTown&language=it")]
        [TestCase("/api/eat-and-drink/card-list?municipality=TestTown&language=it")]
        [TestCase("/api/entertainment-leisure/card-list?municipality=TestTown&language=it")]
        [TestCase("/api/nature/card-list?municipality=TestTown&language=it")]
        [TestCase("/api/organizations/card-list?municipality=TestTown&language=it")]
        [TestCase("/api/public-event/card-list?municipality=TestTown&language=it")]
        [TestCase("/api/routes/card-list?municipality=TestTown&language=it")]
        [TestCase("/api/services/card-list?municipality=TestTown&language=it")]
        [TestCase("/api/shopping/card-list?municipality=TestTown&language=it")]
        public async Task Get_CardList_Returns_Data(string url)
        {
            var client = _client ?? throw new InvalidOperationException("HTTP client was not initialized.");
            var response = await client.GetAsync(url);

            NUnitAssert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(content);
            NUnitAssert.That(document.RootElement.ValueKind, Is.EqualTo(JsonValueKind.Array));

            if (document.RootElement.GetArrayLength() > 0)
            {
                var firstElement = document.RootElement[0];
                NUnitAssert.That(firstElement.ValueKind, Is.EqualTo(JsonValueKind.Object));
                NUnitAssert.That(firstElement.EnumerateObject().Any(), Is.True);
            }
        }
    }
}
