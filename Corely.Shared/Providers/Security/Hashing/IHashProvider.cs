namespace Corely.Shared.Providers.Security.Hashing
{
    public interface IHashProvider
    {
        string Hash(string value);

        bool Verify(string value, string hash);
    }
}
