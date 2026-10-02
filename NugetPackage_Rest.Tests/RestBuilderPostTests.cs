using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NugetPackage_Rest.Builders;
using NugetPackage_Rest.Configs;
using NugetPackage_Rest.Exceptions;
using NugetPackage_Rest.Tests.Fakes;
using NugetPackage_Rest.Tests.Models;
using Xunit;

namespace NugetPackage_Rest.Tests
{
    public class RestBuilderPostTests
    {
        private static RestBuilder CreateRestBuilder(HttpClient client)
        {
            return new RestBuilder(
                client,
                Options.Create(new RequestSettings
                {
                    EnableRequestLogs = false
                }));
        }

        [Fact]
        public async Task Post_Success_ReturnsContentAsString()
        {
            using var client = new HttpClient(
                new FakeHttpMessageHandler(_ =>
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("created")
                    }));

            var rest = CreateRestBuilder(client);

            var result = await rest.Post
                .WithoutAuth()
                .WithUri("https://api.example.com", "/orders")
                .WithBody(new { Name = "widget" })
                .GetContentAsStringAsync();

            Assert.Equal("created", result);
        }

        [Fact]
        public async Task Post_Error_ThrowsWithStatusCodeAndBody()
        {
            var body = "{\"error\":\"name is required\"}";

            using var client = new HttpClient(
                new FakeHttpMessageHandler(_ =>
                    new HttpResponseMessage((HttpStatusCode)422)
                    {
                        Content = new StringContent(body)
                    }));

            var rest = CreateRestBuilder(client);

            var exception = await Assert.ThrowsAsync<ApiException>(
                () => rest.Post
                    .WithoutAuth()
                    .WithUri("https://api.example.com", "/orders")
                    .WithBody(new { Name = "" })
                    .GetContentAsStringAsync());

            Assert.Equal(ApiFailureReason.HttpError, exception.Reason);
            Assert.Equal(422, exception.StatusCode);
            Assert.Equal(body, exception.ResponseBody);
        }

        [Fact]
        public async Task Post_ValidJson_DeserializesIntoTargetType()
        {
            using var client = new HttpClient(
                new FakeHttpMessageHandler(_ =>
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(
                            "{\"id\":7,\"name\":\"widget\"}")
                    }));

            var rest = CreateRestBuilder(client);

            var result = await rest.Post
                .WithoutAuth()
                .WithUri("https://api.example.com", "/orders")
                .WithBody(new { Name = "widget" })
                .DeserializeWithAsync<TestOrder>();

            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            Assert.Equal("widget", result.Name);
        }

        [Fact]
        public async Task Post_WithoutBody_SendsSuccessfully()
        {
            using var client = new HttpClient(
                new FakeHttpMessageHandler(_ =>
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("ok")
                    }));

            var rest = CreateRestBuilder(client);

            var result = await rest.Post
                .WithoutAuth()
                .WithUri("https://api.example.com", "/orders/sync")
                .WithoutBody()
                .GetContentAsStringAsync();

            Assert.Equal("ok", result);
        }

        [Fact]
        public async Task Post_WithFormUrlEncoded_SendsCorrectContentType()
        {
            HttpRequestMessage? capturedRequest = null;

            using var client = new HttpClient(
                new FakeHttpMessageHandler(request =>
                {
                    capturedRequest = request;

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("ok")
                    };
                }));

            var rest = CreateRestBuilder(client);

            await rest.Post
                .WithoutAuth()
                .WithUri("https://api.example.com", "/auth")
                .WithFormUrlEncoded(new Dictionary<string, string>
                {
                    ["user"] = "usuario1",
                    ["pwd"] = "secret"
                })
                .GetContentAsStringAsync();

            Assert.NotNull(capturedRequest);
            Assert.NotNull(capturedRequest.Content);

            Assert.Equal(
                "application/x-www-form-urlencoded",
                capturedRequest.Content.Headers.ContentType?.MediaType);

            var body = await capturedRequest.Content.ReadAsStringAsync();

            Assert.Equal("user=usuario1&pwd=secret", body);
        }
    }
}