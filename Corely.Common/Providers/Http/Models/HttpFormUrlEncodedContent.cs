namespace Corely.Common.Providers.Http.Models
{
    public sealed class HttpFormUrlEncodedContent(
        Dictionary<string, string> content)
        : HttpDictionaryContentBase(content)
    {
    }
}
