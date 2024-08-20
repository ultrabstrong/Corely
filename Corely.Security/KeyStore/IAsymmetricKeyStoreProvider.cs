namespace Corely.Security.KeyStore
{
    public interface IAsymmetricKeyStoreProvider
    {
        (string PublicKey, string PrivateKey) GetCurrentKeys();
        int GetCurrentVersion();
        (string PublicKey, string PrivateKey) Get(int version);
    }
}
