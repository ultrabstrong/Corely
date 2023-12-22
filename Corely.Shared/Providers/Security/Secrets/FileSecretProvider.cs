namespace Corely.Shared.Providers.Security.Secrets
{
    public class FileSecretProvider : ISecretProvider
    {
        private readonly string _filePath;
        private readonly int _version = 1;
        public FileSecretProvider(string filePath)
        {
            _filePath = filePath;
        }
        public (string, int) GetCurrentVersion() => (GetFileContents(), _version);
        public string Get(int version) => GetFileContents();
        protected virtual string GetFileContents() => File.ReadAllText(_filePath);
    }
}
