using Corely.Common.Providers.Security.Hashing;

namespace Corely.Common.Providers.Security.Factories
{
    public class HashProviderFactory : IHashProviderFactory
    {
        public IHashProvider Create()
        {
            throw new NotImplementedException();
        }

        public IHashProvider CreateToVerify(string hash)
        {
            throw new NotImplementedException();
        }
    }
}
