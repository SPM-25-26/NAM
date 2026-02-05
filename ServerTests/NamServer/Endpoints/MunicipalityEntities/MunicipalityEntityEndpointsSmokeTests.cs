using Domain.Entities.MunicipalityEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using nam.Server.Endpoints.MunicipalityEntities;
using nam.Server.Services.Interfaces.MunicipalityEntities;
using NSubstitute;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.NamServer.Endpoints.MunicipalityEntities
{
    [TestFixture]
    public class MunicipalityEntityEndpointsSmokeTests
    {
        [Test]
        public async Task ArtCulture_GetCardList_ReturnsOk()
        {
            var sample = new ArtCultureNatureCard { EntityName = "Art", BadgeText = "Badge", ImagePath = "image.png" };

            await AssertCardListOkAsync<ArtCultureNatureCard, ArtCultureNatureDetail>(
                service => ArtCultureEndpoints.GetCardList(service, "TestTown", "it"),
                sample);
        }

        [Test]
        public async Task Article_GetCardList_ReturnsOk()
        {
            var sample = new ArticleCard { EntityName = "Article", BadgeText = "Badge", ImagePath = "image.png" };

            await AssertCardListOkAsync<ArticleCard, ArticleDetail>(
                service => ArticleEndpoint.GetCardList(service, "TestTown", "it"),
                sample);
        }

        [Test]
        public async Task EatAndDrink_GetCardList_ReturnsOk()
        {
            var sample = new EatAndDrinkCard { EntityName = "Eat", BadgeText = "Badge", ImagePath = "image.png" };

            await AssertCardListOkAsync<EatAndDrinkCard, EatAndDrinkDetail>(
                service => EatAndDrinkEndpoints.GetCardList(service, "TestTown", "it"),
                sample);
        }

        [Test]
        public async Task EntertainmentLeisure_GetCardList_ReturnsOk()
        {
            var sample = new EntertainmentLeisureCard { EntityName = "Fun", BadgeText = "Badge", ImagePath = "image.png" };

            await AssertCardListOkAsync<EntertainmentLeisureCard, EntertainmentLeisureDetail>(
                service => EntertainmentLeisureEndpoints.GetCardList(service, "TestTown", "it"),
                sample);
        }

        [Test]
        public async Task Nature_GetCardList_ReturnsOk()
        {
            var sample = new Nature { EntityName = "Nature", BadgeText = "Badge", ImagePath = "image.png" };

            await AssertCardListOkAsync<Nature, ArtCultureNatureDetail>(
                service => NatureEndpoints.GetCardList(service, "TestTown", "it"),
                sample);
        }

        [Test]
        public async Task Organization_GetCardList_ReturnsOk()
        {
            var sample = new OrganizationCard { TaxCode = "TAX001", EntityName = "Org" };

            await AssertCardListOkAsync<OrganizationCard, OrganizationMobileDetail>(
                service => OrganizationEndpoints.GetCardList(service, "TestTown", "it"),
                sample);
        }

        [Test]
        public async Task PublicEvent_GetCardList_ReturnsOk()
        {
            var sample = new PublicEventCard
            {
                EntityName = "Event",
                BadgeText = "Badge",
                ImagePath = "image.png",
                Address = "Address",
                Date = "2026-02-05"
            };

            await AssertCardListOkAsync<PublicEventCard, PublicEventMobileDetail>(
                service => PublicEventEndpoint.GetCardList(service, "TestTown", "it"),
                sample);
        }

        [Test]
        public async Task Route_GetCardList_ReturnsOk()
        {
            var sample = new RouteCard { EntityName = "Route", BadgeText = "Badge", ImagePath = "image.png" };

            await AssertCardListOkAsync<RouteCard, RouteDetail>(
                service => RouteEndpoints.GetCardList(service, "TestTown", "it"),
                sample);
        }

        [Test]
        public async Task Service_GetCardList_ReturnsOk()
        {
            var sample = new ServiceCard { EntityName = "Service", BadgeText = "Badge", ImagePath = "image.png" };

            await AssertCardListOkAsync<ServiceCard, ServiceDetail>(
                service => ServiceEndpoints.GetCardList(service, "TestTown", "it"),
                sample);
        }

        [Test]
        public async Task Shopping_GetCardList_ReturnsOk()
        {
            var sample = new ShoppingCard { EntityName = "Shop", BadgeText = "Badge", ImagePath = "image.png" };

            await AssertCardListOkAsync<ShoppingCard, ShoppingCardDetail>(
                service => ShoppingEndpoints.GetCardList(service, "TestTown", "it"),
                sample);
        }

        private static async Task AssertCardListOkAsync<TCard, TDetail>(
            Func<IMunicipalityEntityService<TCard, TDetail>, Task<IResult>> action,
            TCard sampleCard)
        {
            var service = Substitute.For<IMunicipalityEntityService<TCard, TDetail>>();
            IEnumerable<TCard> cards = new List<TCard> { sampleCard };

            service.GetCardListAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(cards);

            var result = await action(service);

            var okResult = result as Ok<IEnumerable<TCard>>;
            NUnitAssert.That(okResult, Is.Not.Null);
            NUnitAssert.That(okResult!.Value, Is.SameAs(cards));
        }
    }
}
