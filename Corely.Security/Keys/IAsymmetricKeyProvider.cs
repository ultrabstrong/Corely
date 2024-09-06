namespace Corely.Security.Keys
{
    public interface IAsymmetricKeyProvider
    {
        (string PublicKey, string PrivateKey) CreateKeys();
        bool IsKeyValid(string publicKey, string privateKey);
    }
}
