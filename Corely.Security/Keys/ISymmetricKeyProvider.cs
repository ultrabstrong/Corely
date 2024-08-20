namespace Corely.Security.Keys
{
    public interface ISymmetricKeyProvider
    {
        string CreateKey();

        bool IsKeyValid(string key);
    }
}
