namespace Corely.Security.KeyStore
{
    public class FileAsymmetricKeyStoreProvider
    {
        private readonly string _filePath;
        private readonly int _version = 1;

        public FileAsymmetricKeyStoreProvider(string filePath)
        {
            _filePath = filePath;
        }

        public string GetCurrentKey() => GetFileContents();

        protected virtual string GetFileContents() => File.ReadAllText(_filePath);

        public int GetCurrentVersion() => _version;

        public string Get(int version) => GetFileContents();
    }
}
