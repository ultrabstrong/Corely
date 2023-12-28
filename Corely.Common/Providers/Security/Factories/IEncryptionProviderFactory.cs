using Corely.Common.Providers.Security.Encryption;

namespace Corely.Common.Providers.Security.Factories
{
    public interface IEncryptionProviderFactory : IProviderFactory<IEncryptionProvider>
    {
        IEncryptionProvider GetProviderForDecrypting(string value);
    }
}
