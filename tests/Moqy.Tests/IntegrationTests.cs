/*using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Moqy.Tests
{
    public class IntegrationTests : IClassFixture<CustomWebApplicationFactory>, IDisposable
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public IntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_EndpointReturnsSuccessAndCorrectContentType()
        {
            // Arrange
            var url = "/api/mock/stream?totalItems=1";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("text/event-stream", response.Content.Headers.ContentType?.MediaType);
        }

        [Fact]
        public async Task Get_EndpointReturnsCorrectResponse()
        {
            // Arrange
            var url = "/api/mock/stream?totalItems=1";

            // Act
            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.StartsWith("data:", content);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}*/