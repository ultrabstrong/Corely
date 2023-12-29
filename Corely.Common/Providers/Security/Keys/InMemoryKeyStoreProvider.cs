using Corely.Common.Providers.Security.Exceptions;

namespace Corely.Common.Providers.Security.Keys
{
    public class InMemoryKeyStoreProvider : IKeyStoreProvider
    {
        private readonly Dictionary<int, string> _keys = [];
        private int _version = 0;

        public InMemoryKeyStoreProvider(string key)
        {
            Add(key);
        }

        public void Add(string key)
        {
            _keys.Add(++_version, key);
        }

        public (string, int) GetCurrentVersion() => (_keys[_version], _version);

        public string Get(int version)
        {
            if (!_keys.TryGetValue(version, out var key))
            {
                throw new KeyStoreProviderException($"Key version {version} is invalid")
                {
                    Reason = KeyStoreProviderException.ErrorReason.InvalidVersion
                };
            }

            return key;
        }
    }
}
