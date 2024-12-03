namespace Corely.Security.KeyStore;

public class FileSymmetricKeyStoreProvider : ISymmetricKeyStoreProvider
{
    private readonly string _filePath;
    private readonly int _version = 1;

    public FileSymmetricKeyStoreProvider(string filePath)
    {
        _filePath = filePath;
    }

    public int GetCurrentVersion() => _version;

    public string Get(int version) => GetFileContents();

    public string GetCurrentKey() => GetFileContents();

    protected virtual string GetFileContents() => File.ReadAllText(_filePath);
}
