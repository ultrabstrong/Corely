namespace Corely.Shared.Providers.Security.Encryption
{
    public interface IEncryptionProvider
    {
        string Encrypt(string value);
        string Decrypt(string value);
        string ReEncryptWithCurrentKey(string value, bool skipIfAlreadyCurrent = true);
    }
}
