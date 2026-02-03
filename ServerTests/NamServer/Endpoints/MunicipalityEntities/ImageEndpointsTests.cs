using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using nam.Server.Endpoints.MunicipalityEntities;
using NSubstitute;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace nam.ServerTests.NamServer.Endpoints.MunicipalityEntities
{
    [TestFixture]
    public class ImageEndpointsTests
    {
        [Test]
        public async Task GetExternalImage_ReturnsBadRequest_WhenImagePathIsBlank()
        {
            var configuration = Substitute.For<IConfiguration>();
            var httpClientFactory = Substitute.For<IHttpClientFactory>();

            var result = await ImageEndpoints.GetExternalImage(configuration, httpClientFactory, " ");

            var badRequest = result as BadRequest<string>;
            NUnitAssert.That(badRequest, Is.Not.Null);
            NUnitAssert.That(badRequest!.Value, Is.EqualTo("Image path is required."));
        }

        [Test]
        public async Task GetExternalImage_ReturnsBadRequest_WhenBaseUrlIsMissing()
        {
            var configuration = Substitute.For<IConfiguration>();
            configuration["ExternalImageBaseUrl"].Returns(string.Empty);
            var httpClientFactory = Substitute.For<IHttpClientFactory>();

            var result = await ImageEndpoints.GetExternalImage(configuration, httpClientFactory, "image.png");

            var badRequest = result as BadRequest<string>;
            NUnitAssert.That(badRequest, Is.Not.Null);
            NUnitAssert.That(badRequest!.Value, Is.EqualTo("ExternalImageBaseUrl not configured."));
        }

        [Test]
        public async Task GetExternalImage_ReturnsBadRequest_WhenBaseUrlIsInvalid()
        {
            var configuration = Substitute.For<IConfiguration>();
            configuration["ExternalImageBaseUrl"].Returns("not-a-url");
            var httpClientFactory = Substitute.For<IHttpClientFactory>();

            var result = await ImageEndpoints.GetExternalImage(configuration, httpClientFactory, "image.png");

            var badRequest = result as BadRequest<string>;
            NUnitAssert.That(badRequest, Is.Not.Null);
            NUnitAssert.That(badRequest!.Value, Is.EqualTo("Invalid ExternalImageBaseUrl configuration."));
        }

        [Test]
        public async Task GetExternalImage_ReturnsBadRequest_WhenImagePathIsInvalid()
        {
            var configuration = Substitute.For<IConfiguration>();
            configuration["ExternalImageBaseUrl"].Returns("https://example.com/");
            var httpClientFactory = Substitute.For<IHttpClientFactory>();

            var result = await ImageEndpoints.GetExternalImage(configuration, httpClientFactory, "http://");

            var badRequest = result as BadRequest<string>;
            NUnitAssert.That(badRequest, Is.Not.Null);
            NUnitAssert.That(badRequest!.Value, Is.EqualTo("Invalid image path."));
        }

        [Test]
        public async Task GetExternalImage_ReturnsNotFound_WhenImageRequestFails()
        {
            var configuration = Substitute.For<IConfiguration>();
            configuration["ExternalImageBaseUrl"].Returns("https://example.com/");
            var response = new HttpResponseMessage(HttpStatusCode.NotFound);
            using var handler = new StubHttpMessageHandler(response);
            using var client = new HttpClient(handler);
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient().Returns(client);

            var result = await ImageEndpoints.GetExternalImage(configuration, httpClientFactory, "image.png");

            NUnitAssert.That(result, Is.InstanceOf<NotFound>());
        }

        [Test]
        public async Task GetExternalImage_ReturnsFile_WithResponseContentType()
        {
            var configuration = Substitute.For<IConfiguration>();
            configuration["ExternalImageBaseUrl"].Returns("https://example.com/");
            using var contentStream = new MemoryStream(Encoding.UTF8.GetBytes("test image data"));
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(contentStream)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            using var handler = new StubHttpMessageHandler(response);
            using var client = new HttpClient(handler);
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient().Returns(client);

            var result = await ImageEndpoints.GetExternalImage(configuration, httpClientFactory, "image.jpg");

            var fileResult = result as FileStreamHttpResult;
            NUnitAssert.That(fileResult, Is.Not.Null);
            NUnitAssert.That(fileResult!.ContentType, Is.EqualTo("image/jpeg"));
        }

        private sealed class StubHttpMessageHandler : HttpMessageHandler
        {
            private readonly HttpResponseMessage _response;

            public StubHttpMessageHandler(HttpResponseMessage response)
            {
                _response = response;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_response);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _response.Dispose();
                }

                base.Dispose(disposing);
            }
        }
    }
}
