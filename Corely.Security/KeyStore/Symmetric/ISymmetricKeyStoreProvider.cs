namespace Corely.Security.KeyStore.Symmetric
{
    public interface ISymmetricKeyStoreProvider
    {
        (string Key, int Version) GetCurrentVersion();
        string Get(int version);
    }
}
