namespace Corely.Security.Encryption.Models
{
    public interface ISymmetricEncryptedValue
    {
        string Secret { get; }
        void Set(string decryptedValue);
        string GetDecrypted();
        void ReEncrypt();
    }
}
