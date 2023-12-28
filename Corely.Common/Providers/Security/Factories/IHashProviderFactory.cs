using Corely.Common.Providers.Security.Hashing;

namespace Corely.Common.Providers.Security.Factories
{
    public interface IHashProviderFactory : IProviderFactory<IHashProvider>
    {
        IHashProvider GetProviderToVerify(string hash);
    }
}
