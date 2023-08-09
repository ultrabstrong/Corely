namespace Corely.Models.Security
{
    public interface IEncryptedValue
    {
        string Secret { get; }
        void Set(string decryptedValue);
        string Get();
    }
}
