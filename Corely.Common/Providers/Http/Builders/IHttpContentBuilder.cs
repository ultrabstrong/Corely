using Corely.Common.Providers.Http.Models;

namespace Corely.Common.Providers.Http.Builders
{
    public interface IHttpContentBuilder
    {
        HttpContent Build<T>(IHttpContent<T> content);
    }
}
