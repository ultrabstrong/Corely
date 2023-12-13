namespace Corely.Shared.Providers.Security
{
    public class InMemorySecretProvider : ISecretProvider
    {
        private readonly string _secret;
        public InMemorySecretProvider(string secret)
        {
            _secret = secret;
        }
        public string Get() => _secret;
    }
}
