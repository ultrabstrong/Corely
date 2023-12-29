namespace Corely.Common.Providers.Http.Models
{
    public sealed class HttpMultipartFormDataContent(
        Dictionary<string, string> content)
        : HttpDictionaryContentBase(content)
    {
    }
}
