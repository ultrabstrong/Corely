using Corely.Common.Http.Models;

namespace Corely.Common.Http.Builders
{
    public interface IHttpContentBuilder
    {
        HttpContent Build<T>(IHttpContent<T> content);
    }
}
