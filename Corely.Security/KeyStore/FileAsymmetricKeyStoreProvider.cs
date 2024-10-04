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

        public (string PublicKey, string PrivateKey) Get(int version)
        {
            var keys = GetFileContents().Split(Environment.NewLine);
            return (keys[0], keys[1]);
        }

        public (string PublicKey, string PrivateKey) GetCurrentKeys()
        {
            var keys = GetFileContents().Split(Environment.NewLine);
            return (keys[0], keys[1]);
        }

        protected virtual string GetFileContents() => File.ReadAllText(_filePath);
    }
}
