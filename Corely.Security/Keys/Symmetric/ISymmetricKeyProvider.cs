namespace Corely.Security.Keys.Symmetric
{
    public interface ISymmetricKeyProvider
    {
        string CreateKey();

        bool IsKeyValid(string key);
    }
}
