namespace Corely.Security.Keys.Asymmetric
{
    public interface IAsymmetricKeyProvider
    {
        (string PublicKey, string PrivateKey) CreateKeyPair();
        bool IsKeyValid(string publicKey, string privateKey);
    }
}
