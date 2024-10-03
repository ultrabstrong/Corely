namespace Corely.Security.Keys
{
    public interface IAsymmetricSignatureKeyProvider
    {
        (string PublicKey, string PrivateKey) CreateKeys();
        bool IsKeyValid(string publicKey, string privateKey);
    }
}
