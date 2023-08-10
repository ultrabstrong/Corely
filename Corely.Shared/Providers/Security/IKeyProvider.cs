namespace Corely.Shared.Providers.Security
{
    public interface IKeyProvider
    {
        string GetKey();

        bool IsKeyValid(string key);
    }
}
