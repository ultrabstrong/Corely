namespace Corely.Common.Providers.Http.Models
{
    public class HttpStringContentBase(
        string content)
        : IHttpContent<string>
    {
        public string Content { get; } = content;
    }
}
