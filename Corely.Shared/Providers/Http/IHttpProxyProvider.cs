using Corely.Shared.Providers.Http.Models;

namespace Corely.Shared.Providers.Http
{
    public interface IHttpProxyProvider
    {
        Task<HttpResponseMessage> SendRequestForHttpResponse<T>(
            HttpSendRequest request,
            IHttpContent<T>? httpContent = null);
    }
}
