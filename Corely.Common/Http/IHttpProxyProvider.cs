using Corely.Common.Http.Models;

namespace Corely.Common.Http;

public interface IHttpProxyProvider
{
    Task<HttpResponseMessage> SendRequestForHttpResponse<T>(
        HttpSendRequest request,
        IHttpContent<T>? httpContent = null);
}
