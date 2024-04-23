using Corely.Security.Encryption.Providers;

namespace Corely.Security.Encryption.Factories
{
    public interface IEncryptionProviderFactory : IProviderFactory<IEncryptionProvider>
    {
        IEncryptionProvider GetProviderForDecrypting(string value);
    }
}
