using Domain.Entities.MunicipalityEntities;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
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
        private const string MunicipalityName = "TestTown";
        private static readonly object s_seedLock = new();
        private NamTestFactory? _factory;
        private HttpClient? _client;

        [OneTimeSetUp]
        public void Setup()
        {
            _factory = new NamTestFactory();
            SeedMunicipalityData();
            _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Test");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }

        [TestCase("/api/art-culture/card-list?municipality=TestTown&language=it", "entityName")]
        [TestCase("/api/article/card-list?municipality=TestTown&language=it", "entityName")]
        [TestCase("/api/eat-and-drink/card-list?municipality=TestTown&language=it", "entityName")]
        [TestCase("/api/entertainment-leisure/card-list?municipality=TestTown&language=it", "entityName")]
        [TestCase("/api/nature/card-list?municipality=TestTown&language=it", "entityName")]
        [TestCase("/api/organizations/card-list?municipality=TestTown&language=it", "entityName")]
        [TestCase("/api/public-event/card-list?municipality=TestTown&language=it", "entityName")]
        [TestCase("/api/routes/card-list?municipality=TestTown&language=it", "entityName")]
        [TestCase("/api/services/card-list?municipality=TestTown&language=it", "entityName")]
        [TestCase("/api/shopping/card-list?municipality=TestTown&language=it", "entityName")]
        [TestCase("/api/organizations/municipalities?search=TestTown&language=it", "legalName")]
        public async Task Get_CardList_Returns_Data(string url, string expectedField)
        {
            var client = _client ?? throw new InvalidOperationException("HTTP client was not initialized.");
            var response = await client.GetAsync(url);

            NUnitAssert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(content);

            
            NUnitAssert.That(document.RootElement.ValueKind, Is.EqualTo(JsonValueKind.Array));

            
            NUnitAssert.That(document.RootElement.GetArrayLength(), Is.GreaterThan(0), "L'API ha restituito un array vuoto, il Seed non ha funzionato o il filtro fallisce.");

            NUnitAssert.That(content, Does.Contain(expectedField));
        }

        private void SeedMunicipalityData()
        {
            var factory = _factory ?? throw new InvalidOperationException("Factory was not initialized.");
            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            lock (s_seedLock)
            {
                if (context.MunicipalityCards.Any(card =>
                        card.LegalName != null && card.LegalName.Contains(MunicipalityName)))
                {
                    return;
                }

                var municipalityData = CreateMunicipalityData();

                var artCultureDetail = new ArtCultureNatureDetail
                {
                    Identifier = Guid.NewGuid(),
                    OfficialName = "Art Culture",
                    MunicipalityData = municipalityData
                };
                var artCultureCard = new ArtCultureNatureCard
                {
                    EntityId = Guid.NewGuid(),
                    EntityName = "Art Culture",
                    BadgeText = "Badge",
                    ImagePath = "image.png",
                    Detail = artCultureDetail
                };

                var articleDetail = new ArticleDetail
                {
                    Identifier = Guid.NewGuid(),
                    Title = "Article Title",
                    Script = "Script",
                    ImagePath = "image.png",
                    UpdatedAt = DateTime.UtcNow,
                    MunicipalityData = municipalityData
                };
                var articleCard = new ArticleCard
                {
                    EntityId = Guid.NewGuid(),
                    EntityName = "Article",
                    BadgeText = "Badge",
                    ImagePath = "image.png",
                    Detail = articleDetail
                };

                var eatAndDrinkDetail = new EatAndDrinkDetail
                {
                    Identifier = Guid.NewGuid(),
                    OfficialName = "Eat & Drink",
                    MunicipalityData = municipalityData
                };
                var eatAndDrinkCard = new EatAndDrinkCard
                {
                    EntityId = Guid.NewGuid(),
                    EntityName = "Eat & Drink",
                    BadgeText = "Badge",
                    ImagePath = "image.png",
                    Detail = eatAndDrinkDetail
                };

                var entertainmentDetail = new EntertainmentLeisureDetail
                {
                    Identifier = Guid.NewGuid(),
                    OfficialName = "Entertainment",
                    MunicipalityData = municipalityData
                };
                var entertainmentCard = new EntertainmentLeisureCard
                {
                    EntityId = Guid.NewGuid(),
                    EntityName = "Entertainment",
                    BadgeText = "Badge",
                    ImagePath = "image.png",
                    Detail = entertainmentDetail
                };

                var natureDetail = new ArtCultureNatureDetail
                {
                    Identifier = Guid.NewGuid(),
                    OfficialName = "Nature",
                    MunicipalityData = municipalityData
                };
                var natureCard = new Nature
                {
                    EntityId = Guid.NewGuid(),
                    EntityName = "Nature",
                    BadgeText = "Badge",
                    ImagePath = "image.png",
                    Detail = natureDetail
                };

                var organizationDetail = new OrganizationMobileDetail
                {
                    TaxCode = "ORG001",
                    LegalName = "Organization",
                    MunicipalityData = municipalityData
                };
                var organizationCard = new OrganizationCard
                {
                    TaxCode = "ORG001",
                    EntityName = "Organization",
                    Detail = organizationDetail
                };

                var publicEventDetail = new PublicEventMobileDetail
                {
                    Identifier = Guid.NewGuid(),
                    Title = "Public Event",
                    MunicipalityData = municipalityData
                };

                var routeDetail = new RouteDetail
                {
                    Identifier = Guid.NewGuid(),
                    Name = "Route",
                    MunicipalityData = municipalityData
                };
                var routeCard = new RouteCard
                {
                    EntityId = routeDetail.Identifier,
                    EntityName = "Route",
                    BadgeText = "Badge",
                    ImagePath = "image.png",
                    Detail = routeDetail
                };
                var publicEventCard = new PublicEventCard
                {
                    EntityId = publicEventDetail.Identifier,
                    EntityName = "Public Event",
                    BadgeText = "Badge",
                    ImagePath = "image.png",
                    Address = "Address",
                    Date = "2026-02-05",
                    Detail = publicEventDetail
                };

                var serviceDetail = new ServiceDetail
                {
                    Identifier = Guid.NewGuid(),
                    Name = "Service",
                    MunicipalityData = municipalityData
                };
                var serviceCard = new ServiceCard
                {
                    EntityId = serviceDetail.Identifier,
                    EntityName = "Service",
                    BadgeText = "Badge",
                    ImagePath = "image.png",
                    Detail = serviceDetail
                };

                var shoppingDetail = new ShoppingCardDetail
                {
                    Identifier = Guid.NewGuid(),
                    OfficialName = "Shopping",
                    MunicipalityData = municipalityData
                };
                var shoppingCard = new ShoppingCard
                {
                    EntityId = shoppingDetail.Identifier,
                    EntityName = "Shopping",
                    BadgeText = "Badge",
                    ImagePath = "image.png",
                    Detail = shoppingDetail
                };

                var municipalityCard = new MunicipalityCard
                {
                    LegalName = $"{MunicipalityName} Municipality",
                    ImagePath = "image.png"
                };

                context.AddRange(
                    municipalityData,
                    artCultureDetail,
                    artCultureCard,
                    natureDetail,
                    natureCard,
                    articleDetail,
                    articleCard,
                    eatAndDrinkDetail,
                    eatAndDrinkCard,
                    entertainmentDetail,
                    entertainmentCard,
                    organizationDetail,
                    organizationCard,
                    publicEventDetail,
                    publicEventCard,
                    routeDetail,
                    routeCard,
                    serviceDetail,
                    serviceCard,
                    shoppingDetail,
                    shoppingCard,
                    municipalityCard);

                context.SaveChanges();
            }
        }

        private MunicipalityForLocalStorageSetting CreateMunicipalityData()
        {
            return new MunicipalityForLocalStorageSetting
            {
                Name = MunicipalityName,
                LogoPath = "logo.png"
            };
        }
    }
}