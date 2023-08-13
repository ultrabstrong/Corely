using Corely.Shared.Models.Http;

namespace Corely.Shared.Providers.Proxies
{
    public interface IHttpProxyProvider
    {
        bool IsConnected { get; }

        void Connect(string host);

        void Disconnect();

        HttpResponseMessage SendRequestForHttpResponse(string requestUri, HttpMethod method, HttpContent content, Dictionary<string, string> headers, IHttpParameters parameters);
    }
}
