using Corely.Shared.Providers.Http.Models;

namespace Corely.Shared.Providers.Http.Builders
{
    public interface IHttpContentBuilder
    {
        HttpContent Build<T>(IHttpContent<T> content);
    }
}
