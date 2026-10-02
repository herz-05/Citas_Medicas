using System;
using System.Net.Http;
using NugetPackage_Rest.Exceptions;
using Xunit;

namespace NugetPackage_Rest.Tests
{
    public class ApiExceptionTests
    {
        [Fact]
        public void Constructor_SetsAllProperties()
        {
            var inner = new InvalidOperationException("boom");
            var uri = new Uri("https://api.example.com/orders");
            var body = "{\"error\":\"invalid\"}";

            var exception = new ApiException(
                "La petición falló con código 422.",
                ApiFailureReason.HttpError,
                inner,
                422,
                body,
                HttpMethod.Post,
                uri);

            Assert.Equal(ApiFailureReason.HttpError, exception.Reason);
            Assert.Equal(422, exception.StatusCode);
            Assert.Equal(body, exception.ResponseBody);
            Assert.Equal(HttpMethod.Post, exception.RequestMethod);
            Assert.Equal(uri, exception.RequestUri);
            Assert.Same(inner, exception.InnerException);
            Assert.Contains("422", exception.Message);
        }

        [Fact]
        public void Constructor_AllowsOptionalParametersToBeOmitted()
        {
            var exception = new ApiException(
                "La petición tardó demasiado.",
                ApiFailureReason.Timeout);

            Assert.Equal(ApiFailureReason.Timeout, exception.Reason);
            Assert.Null(exception.StatusCode);
            Assert.Null(exception.ResponseBody);
            Assert.Null(exception.RequestMethod);
            Assert.Null(exception.RequestUri);
            Assert.Null(exception.InnerException);
        }
    }
}