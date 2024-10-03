namespace Corely.Security.Keys
{
    public interface IAsymmetricEncryptionKeyProvider
    {
        (string PublicKey, string PrivateKey) CreateKeys();
        bool IsKeyValid(string publicKey, string privateKey);
    }
}
