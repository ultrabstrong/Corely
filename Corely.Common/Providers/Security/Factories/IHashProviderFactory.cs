using Corely.Common.Providers.Security.Hashing;

namespace Corely.Common.Providers.Security.Factories
{
    public interface IHashProviderFactory
    {
        IHashProvider Create(string providerCode);

        IHashProvider CreateToVerify(string hash);
    }
}
