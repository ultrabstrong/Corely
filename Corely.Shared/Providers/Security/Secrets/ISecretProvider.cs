namespace Corely.Shared.Providers.Security.Secrets
{
    public interface ISecretProvider
    {
        (string, int) GetCurrentVersion();
        string Get(int version);
    }
}
