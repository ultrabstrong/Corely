using Corely.Common.Providers.Security.Hashing;

namespace Corely.Common.Providers.Security.Factories
{
    public interface IHashProviderFactory
    {
        IHashProvider Create();

        IHashProvider CreateToVerify(string hash);
    }
}
