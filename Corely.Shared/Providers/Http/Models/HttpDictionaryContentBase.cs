namespace Corely.Shared.Providers.Http.Models
{
    public class HttpDictionaryContentBase : IHttpContent<Dictionary<string, string>>
    {
        public Dictionary<string, string> Content { get; }

        public HttpDictionaryContentBase(Dictionary<string, string> content)
        {
            ArgumentNullException.ThrowIfNull(content, nameof(content));
            Content = content;
        }
    }
}
