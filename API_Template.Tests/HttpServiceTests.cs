using API_Template.Services;
using Moq;
using Moq.Protected;
using System.Net;

namespace API_Template.Tests
{
    public class HttpServiceTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly HttpService _httpService;

        public HttpServiceTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpService = new HttpService(_httpClientFactoryMock.Object, "https://your-api-base-url.com");
        }

        [Fact]
        public async Task GetAsync_ReturnsDeserializedObject()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"Id\": 1, \"Name\": \"Test\"}")
                });

            var client = new HttpClient(handlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var result = await _httpService.GetAsync<Person>("/external-person-endpoint");

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test", result.Name);
        }
        [Fact]
        public async Task GetAsync_ReturnsListOfPersons()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{\"Id\": 1, \"Name\": \"Test Person 1\"}, {\"Id\": 2, \"Name\": \"Test Person 2\"}]")
                });

            var client = new HttpClient(handlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var result = await _httpService.GetAsync<List<Person>>("/external-people-endpoint");

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].Id);
            Assert.Equal("Test Person 1", result[0].Name);
            Assert.Equal(2, result[1].Id);
            Assert.Equal("Test Person 2", result[1].Name);
        }
        [Fact]
        public async Task PostAsync_ReturnsDeserializedObject()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"Id\": 1, \"Name\": \"Test\"}")
                });

            var client = new HttpClient(handlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            var result = await _httpService.PostAsync<Person>("/external-person-endpoint", new Person { Name = "Test" });

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test", result.Name);
        }
        [Fact]
        public async Task PutAsync_ReturnsDeserializedObject()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"Id\": 1, \"Name\": \"Updated Test\"}")
                });

            var client = new HttpClient(handlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var result = await _httpService.PutAsync<Person>("/external-person-endpoint", new Person { Id = 1, Name = "Updated Test" });

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Updated Test", result.Name);
        }
        [Fact]
        public async Task DeleteAsync_Succeeds() // If no exception is thrown, the test passes
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent
                });

            var client = new HttpClient(handlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            await _httpService.DeleteAsync("/external-person-endpoint");
        }
    }
}
