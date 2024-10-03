namespace Corely.Security.KeyStore
{
    public class InMemoryAsymmetricKeyStoreProvider : IAsymmetricEncryptionKeyStoreProvider, IAsymmetricSignatureKeyStoreProvider
    {
        private readonly Dictionary<int, (string, string)> _keys = [];
        private int _version = 0;

        public InMemoryAsymmetricKeyStoreProvider(
            string publicKey, string privateKey)
        {
            Add(publicKey, privateKey);
        }

        public void Add(string publicKey, string privateKey)
        {
            _keys.Add(++_version, (publicKey, privateKey));
        }

        public int GetCurrentVersion() => _version;

        public (string PublicKey, string PrivateKey) Get(int version)
        {
            if (!_keys.TryGetValue(version, out var keys))
            {
                throw new KeyStoreException($"Key version {version} is invalid")
                {
                    Reason = KeyStoreException.ErrorReason.InvalidVersion
                };
            }

            return keys;
        }

        public (string PublicKey, string PrivateKey) GetCurrentKeys() => _keys[_version];
    }
}
