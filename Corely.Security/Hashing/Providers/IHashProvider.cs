namespace Corely.Security.Hashing.Providers
{
    public interface IHashProvider
    {
        string HashTypeCode { get; }
        string Hash(string value);
        bool Verify(string value, string hash);
    }
}
