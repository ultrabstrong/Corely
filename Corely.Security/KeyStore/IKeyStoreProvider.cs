namespace Corely.Security.KeyStore
{
    public interface IKeyStoreProvider
    {
        (string, int) GetCurrentVersion();
        string Get(int version);
    }
}
