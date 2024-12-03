namespace Corely.Common.Http.Models;

public record HttpStringContentBase(string Content)
    : IHttpContent<string>
{
}
