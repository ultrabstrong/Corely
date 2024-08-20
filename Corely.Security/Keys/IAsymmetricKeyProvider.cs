namespace Corely.Security.Keys
{
    public interface IAsymmetricKeyProvider
    {
        (string PublicKey, string PrivateKey) CreateKeyPair();
        bool IsKeyValid(string publicKey, string privateKey);
    }
}
