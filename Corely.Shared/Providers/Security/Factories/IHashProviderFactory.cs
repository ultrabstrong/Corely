using Corely.Shared.Providers.Security.Hashing;

namespace Corely.Shared.Providers.Security.Factories
{
    public interface IHashProviderFactory
    {
        IHashProvider Create();

        IHashProvider CreateToVerify(string hash);
    }
}
