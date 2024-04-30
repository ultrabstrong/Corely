namespace Corely.Security.KeyStore.Symmetric
{
    public class FileSymmetricKeyStoreProvider : ISymmetricKeyStoreProvider
    {
        private readonly string _filePath;
        private readonly int _version = 1;

        public FileSymmetricKeyStoreProvider(string filePath)
        {
            _filePath = filePath;
        }

        public (string Key, int Version) GetCurrentVersion() => (GetFileContents(), _version);
        public string Get(int version) => GetFileContents();
        protected virtual string GetFileContents() => File.ReadAllText(_filePath);
    }
}
