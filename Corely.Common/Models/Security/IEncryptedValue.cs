namespace Corely.Common.Models.Security
{
    public interface IEncryptedValue
    {
        string Secret { get; }
        void Set(string decryptedValue);
        string GetDecrypted();
        void ReEncrypt();
    }
}
