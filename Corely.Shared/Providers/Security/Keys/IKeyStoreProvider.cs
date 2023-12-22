namespace Corely.Shared.Providers.Security.Keys
{
    public interface IKeyStoreProvider
    {
        (string, int) GetCurrentVersion();
        string Get(int version);
    }
}
