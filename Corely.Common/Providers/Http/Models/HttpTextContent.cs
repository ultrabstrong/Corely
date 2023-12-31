namespace Corely.Common.Providers.Http.Models
{
    public sealed class HttpTextContent : HttpStringContentBase
    {
        public HttpTextContent(string content)
            : base(content) { }
    }
}
