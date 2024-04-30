using Corely.Security.Encryption.Providers;

namespace Corely.Security.Encryption.Factories
{
    public interface ISymmetricEncryptionProviderFactory : IProviderFactory<ISymmetricEncryptionProvider>
    {
        ISymmetricEncryptionProvider GetProviderForDecrypting(string value);
    }
}
