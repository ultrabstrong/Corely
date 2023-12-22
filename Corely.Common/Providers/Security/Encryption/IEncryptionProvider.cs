namespace Corely.Common.Providers.Security.Encryption
{
    public interface IEncryptionProvider
    {
        string EncryptionTypeCode { get; }
        string Encrypt(string value);
        string Decrypt(string value);
        string ReEncrypt(string value);
    }
}
