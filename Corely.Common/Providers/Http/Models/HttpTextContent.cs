namespace Corely.Common.Providers.Http.Models
{
    public sealed record HttpTextContent(string Content)
        : HttpStringContentBase(Content)
    {
    }
}
