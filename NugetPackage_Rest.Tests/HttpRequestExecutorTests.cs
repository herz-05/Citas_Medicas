using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NugetPackage_Rest.Builders;
using NugetPackage_Rest.Exceptions;
using NugetPackage_Rest.Tests.Fakes;
using Xunit;

namespace NugetPackage_Rest.Tests
{
    public class HttpRequestExecutorTests
    {
        [Fact]
        public async Task SendAsync_NetworkError_ThrowsApiException()
        {
            var networkError = new HttpRequestException("Connection refused");

            using var client = new HttpClient(
                new FakeHttpMessageHandler(networkError));

            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://api.example.com/orders");

            var exception = await Assert.ThrowsAsync<ApiException>(
                () => HttpRequestExecutor.SendAsync(client, request));

            Assert.Equal(ApiFailureReason.Network, exception.Reason);
            Assert.Same(networkError, exception.InnerException);
        }

        [Fact]
        public async Task SendAsync_Timeout_ThrowsApiException()
        {
            var timeoutError = new TaskCanceledException("Timed out");

            using var client = new HttpClient(
                new FakeHttpMessageHandler(timeoutError));

            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://api.example.com/orders");

            var exception = await Assert.ThrowsAsync<ApiException>(
                () => HttpRequestExecutor.SendAsync(client, request));

            Assert.Equal(ApiFailureReason.Timeout, exception.Reason);
            Assert.Same(timeoutError, exception.InnerException);
        }

        [Fact]
        public async Task SendAsync_Success_ReturnsSameResponse()
        {
            using var response = new HttpResponseMessage(HttpStatusCode.OK);

            using var client = new HttpClient(
                new FakeHttpMessageHandler(_ => response));

            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://api.example.com/orders");

            var result = await HttpRequestExecutor.SendAsync(client, request);

            Assert.Same(response, result);
        }

        [Fact]
        public async Task EnsureSuccessAsync_Error_ThrowsWithStatusCodeAndBody()
        {
            var body = "{\"error\":\"not found\"}";

            using var response = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(body)
            };

            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://api.example.com/orders/42");

            var exception = await Assert.ThrowsAsync<ApiException>(
                () => HttpRequestExecutor.EnsureSuccessAsync(response, request));

            Assert.Equal(ApiFailureReason.HttpError, exception.Reason);
            Assert.Equal(404, exception.StatusCode);
            Assert.Equal(body, exception.ResponseBody);
        }

        [Fact]
        public async Task EnsureSuccessAsync_Success_DoesNotThrow()
        {
            using var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("ok")
            };

            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://api.example.com/orders");

            await HttpRequestExecutor.EnsureSuccessAsync(response, request);
        }
    }
}