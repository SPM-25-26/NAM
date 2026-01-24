using Domain.Entities.MunicipalityEntities;
using Microsoft.AspNetCore.Http.HttpResults;
using nam.Server.Endpoints.MunicipalityEntities;
using nam.Server.Services.Interfaces.MunicipalityEntities;
using NSubstitute;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.NamServer.Endpoints.MunicipalityEntities
{
    [TestFixture]
    public class MunicipalityCardEndpointsTests
    {
        [Test]
        public async Task GetCardDetail_ReturnsNotFound_WhenServiceReturnsNull()
        {
            var service = Substitute.For<IMunicipalityEntityService<MunicipalityCard, MunicipalityHomeInfo>>();
            service.GetCardDetailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns((MunicipalityHomeInfo?)null);

            var result = await MunicipalityCardEndpoints.GetCardDetail(service, "Milano", "it");

            NUnitAssert.That(result, Is.InstanceOf<NotFound>());
        }

        [Test]
        public async Task GetCardDetail_ReturnsOk_WhenServiceReturnsDetail()
        {
            var service = Substitute.For<IMunicipalityEntityService<MunicipalityCard, MunicipalityHomeInfo>>();
            var detail = new MunicipalityHomeInfo { LegalName = "Comune di Milano" };
            service.GetCardDetailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(detail);

            var result = await MunicipalityCardEndpoints.GetCardDetail(service, "Milano", "it");

            var okResult = result as Ok<MunicipalityHomeInfo>;
            NUnitAssert.That(okResult, Is.Not.Null);
            NUnitAssert.That(okResult!.Value, Is.SameAs(detail));
        }
    }
}
