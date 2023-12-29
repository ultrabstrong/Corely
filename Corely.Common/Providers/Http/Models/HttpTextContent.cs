namespace Corely.Common.Providers.Http.Models
{
    public sealed class HttpTextContent(
        string content)
        : HttpStringContentBase(content)
    {
    }
}
