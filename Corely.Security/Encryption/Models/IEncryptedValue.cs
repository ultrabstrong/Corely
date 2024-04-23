namespace Corely.Security.Encryption.Models
{
    public interface IEncryptedValue
    {
        string Secret { get; }
        void Set(string decryptedValue);
        string GetDecrypted();
        void ReEncrypt();
    }
}
