namespace Corely.Security;

public interface IProviderFactory<T>
{
    void AddProvider(string providerCode, T provider);
    void UpdateProvider(string providerCode, T provider);
    T GetDefaultProvider();
    T GetProvider(string providerCode);
    List<(string ProviderCode, Type ProviderType)> ListProviders();
}
