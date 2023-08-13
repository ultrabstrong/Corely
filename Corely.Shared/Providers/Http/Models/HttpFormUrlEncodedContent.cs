namespace Corely.Shared.Providers.Http.Models
{
    public class HttpFormUrlEncodedContent : HttpDictionaryContentBase
    {
        public HttpFormUrlEncodedContent(Dictionary<string, string> content)
            : base(content)
        {
        }
    }
}
