namespace Corely.Security.Encryption.Providers
{
    public interface ISymmetricEncryptionProvider
    {
        string EncryptionTypeCode { get; }
        string Encrypt(string value);
        string Decrypt(string value);
        string ReEncrypt(string value);
    }
}
