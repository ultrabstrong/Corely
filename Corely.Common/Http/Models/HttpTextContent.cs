namespace Corely.Common.Http.Models
{
    public sealed record HttpTextContent(string Content)
        : HttpStringContentBase(Content)
    {
    }
}
