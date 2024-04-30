namespace Corely.Security.KeyStore.Symmetric
{
    public interface ISymmetricKeyStoreProvider
    {
        (string, int) GetCurrentVersion();
        string Get(int version);
    }
}
