using Corely.Shared.Providers.Http.Models;

namespace Corely.Shared.Providers.Http
{
    public interface IHttpProxyProvider
    {
        bool IsConnected { get; }

        void Connect(string host);

        void Disconnect();

        Task<HttpResponseMessage> SendRequestForHttpResponse<T>(
            HttpSendRequest request,
            IHttpContent<T>? httpContent = null);
    }
}
