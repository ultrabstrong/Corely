namespace Corely.Common.Http.Models;

public sealed class HttpFormUrlEncodedContent : HttpDictionaryContentBase
{
    public HttpFormUrlEncodedContent(Dictionary<string, string> content)
        : base(content) { }
}
