namespace Corely.Security.KeyStore
{
    public interface IAsymmetricSignatureKeyStoreProvider
    {
        int GetCurrentVersion();
        (string PublicKey, string PrivateKey) Get(int version);
        (string PublicKey, string PrivateKey) GetCurrentKeys();
    }
}
