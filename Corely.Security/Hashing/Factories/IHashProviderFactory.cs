using Corely.Security.Hashing.Providers;

namespace Corely.Security.Hashing.Factories;

public interface IHashProviderFactory : IProviderFactory<IHashProvider>
{
    IHashProvider GetProviderToVerify(string hash);
}
