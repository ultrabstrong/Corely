namespace Corely.Security.Keys
{
    public interface IKeyProvider
    {
        string CreateKey();

        bool IsKeyValid(string key);
    }
}
