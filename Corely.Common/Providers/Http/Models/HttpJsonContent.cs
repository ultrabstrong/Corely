using System.Text.Json;

namespace Corely.Common.Providers.Http.Models
{
    public sealed record HttpJsonContent(string Content)
        : HttpStringContentBase(Content)
    {
        public HttpJsonContent(object content)
            : this(JsonSerializer.Serialize(content))
        {

        }
    }
}
