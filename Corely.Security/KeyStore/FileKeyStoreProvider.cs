namespace Corely.Security.KeyStore
{
    public class FileKeyStoreProvider : IKeyStoreProvider
    {
        private readonly string _filePath;
        private readonly int _version = 1;

        public FileKeyStoreProvider(string filePath)
        {
            _filePath = filePath;
        }

        public (string, int) GetCurrentVersion() => (GetFileContents(), _version);
        public string Get(int version) => GetFileContents();
        protected virtual string GetFileContents() => File.ReadAllText(_filePath);
    }
}
