namespace Corely.Shared.Providers.Security
{
    public interface IKeyProvider
    {
        string CreateKey();

        bool IsKeyValid(string key);
    }
}
