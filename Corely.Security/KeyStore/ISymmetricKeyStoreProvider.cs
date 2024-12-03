namespace Corely.Security.KeyStore;

public interface ISymmetricKeyStoreProvider
{
    int GetCurrentVersion();
    string Get(int version);
    string GetCurrentKey();
}
