namespace Corely.Common.Providers.Security.Keys
{
    public class FileKeyStoreProvider(
        string filePath)
        : IKeyStoreProvider
    {
        private readonly string _filePath = filePath;
        private readonly int _version = 1;

        public (string, int) GetCurrentVersion() => (GetFileContents(), _version);
        public string Get(int version) => GetFileContents();
        protected virtual string GetFileContents() => File.ReadAllText(_filePath);
    }
}
