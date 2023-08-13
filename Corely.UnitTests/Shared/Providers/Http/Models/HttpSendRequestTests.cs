using Corely.Shared.Providers.Http.Models;

namespace Corely.UnitTests.Shared.Providers.Http.Models
{
    public class HttpSendRequestTests
    {
        private readonly HttpSendRequest _httpSendRequest;

        public HttpSendRequestTests()
        {
            _httpSendRequest = new HttpSendRequest("/v1/entity", HttpMethod.Get);
        }

        [Fact]
        public void CreateHttpRequestMessage_ShouldReturnHttpRequestMessage()
        {
            _httpSendRequest.Parameters = new HttpParameters(
                new Dictionary<string, string>()
                {
                    { "param1", "value1" },
                    { "param2", "value2" }
                },
                new()
                {
                    { "param3", "value3" },
                    { "param4", "value4" }
                }
            );

            _httpSendRequest.Headers = new()
            {
                { "header1", "value1" },
                { "header2", "value2" }
            };

            HttpRequestMessage httpRequestMessage = _httpSendRequest.CreateHttpRequestMessage();

            Assert.Equal("/v1/entity?param1=value1&param2=value2&param3=value3&param4=value4", httpRequestMessage.RequestUri?.ToString());
            Assert.Equal(HttpMethod.Get, httpRequestMessage.Method);
            Assert.Equal("value1", httpRequestMessage.Headers.GetValues("header1").First());
            Assert.Equal("value2", httpRequestMessage.Headers.GetValues("header2").First());
        }
    }
}
