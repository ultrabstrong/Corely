namespace Corely.Security
{
    public interface IKeyProvider
    {
        string GetKey();

        bool IsKeyValid(string key);
    }
}
