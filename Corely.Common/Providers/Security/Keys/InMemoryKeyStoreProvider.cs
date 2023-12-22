using Corely.Common.Providers.Security.Exceptions;

namespace Corely.Common.Providers.Security.Keys
{
    public class InMemoryKeyStoreProvider : IKeyStoreProvider
    {
        private readonly Dictionary<int, string> _secrets = new();
        private int _version = 0;

        public InMemoryKeyStoreProvider(string secret)
        {
            Add(secret);
        }

        public void Add(string secret)
        {
            _secrets.Add(++_version, secret);
        }

        public (string, int) GetCurrentVersion() => (_secrets[_version], _version);

        public string Get(int version)
        {
            if (!_secrets.TryGetValue(version, out var secret))
            {
                throw new SecretProviderException($"Key version {version} is invalid")
                {
                    Reason = SecretProviderException.ErrorReason.InvalidVersion
                };
            }

            return secret;
        }
    }
}
