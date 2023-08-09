namespace Corely.Shared.Security
{
    public interface IKeyProvider
    {
        string GetKey();

        bool IsKeyValid(string key);
    }
}
