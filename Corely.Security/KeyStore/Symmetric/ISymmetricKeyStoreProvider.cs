namespace Corely.Security.KeyStore.Symmetric
{
    public interface ISymmetricKeyStoreProvider
    {
        string GetCurrentKey();
        int GetCurrentVersion();
        string Get(int version);
    }
}
