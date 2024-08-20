namespace Corely.Security.KeyStore
{
    public class InMemorySymmetricKeyStoreProvider : ISymmetricKeyStoreProvider
    {
        private readonly Dictionary<int, string> _keys = [];
        private int _version = 0;

        public InMemorySymmetricKeyStoreProvider(string key)
        {
            Add(key);
        }

        public void Add(string key)
        {
            _keys.Add(++_version, key);
        }

        public string GetCurrentKey() => _keys[_version];

        public int GetCurrentVersion() => _version;

        public string Get(int version)
        {
            if (!_keys.TryGetValue(version, out var key))
            {
                throw new KeyStoreException($"Key version {version} is invalid")
                {
                    Reason = KeyStoreException.ErrorReason.InvalidVersion
                };
            }

            return key;
        }
    }
}
