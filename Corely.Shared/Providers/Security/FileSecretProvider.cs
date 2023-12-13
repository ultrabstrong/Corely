namespace Corely.Shared.Providers.Security
{
    public class FileSecretProvider : ISecretProvider
    {
        private readonly string _filePath;
        public FileSecretProvider(string filePath)
        {
            _filePath = filePath;
        }
        public string Get()
        {
            return File.ReadAllText(_filePath);
        }
    }
}
