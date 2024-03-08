namespace Corely.Common.Providers.Http.Models
{
    public record HttpStringContentBase(string Content)
        : IHttpContent<string>
    {
    }
}
