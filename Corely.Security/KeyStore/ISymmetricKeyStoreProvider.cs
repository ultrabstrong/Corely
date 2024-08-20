namespace Corely.Security.KeyStore
{
    public interface ISymmetricKeyStoreProvider
    {
        string GetCurrentKey();
        int GetCurrentVersion();
        string Get(int version);
    }
}
