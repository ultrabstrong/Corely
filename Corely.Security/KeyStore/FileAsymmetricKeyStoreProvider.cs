namespace Corely.Security.KeyStore
{
    public class FileAsymmetricKeyStoreProvider : IAsymmetricKeyStoreProvider
    {
        private readonly string _filePath;
        private readonly int _version = 1;

        public FileAsymmetricKeyStoreProvider(string filePath)
        {
            _filePath = filePath;
        }

        public int GetCurrentVersion() => _version;

        public (string PublicKey, string PrivateKey) Get(int version) => GetFileContents();

        public (string PublicKey, string PrivateKey) GetCurrentKeys() => GetFileContents();

        protected virtual (string PublicKey, string PrivateKey) GetFileContents()
        {
            var keys = File.ReadAllText(_filePath).Split(Environment.NewLine);
            return (keys[0], keys[1]);
        }
    }
}
