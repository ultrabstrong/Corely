using Corely.Security.Encryption.Providers;

namespace Corely.Security.Encryption.Factories;

public interface IAsymmetricEncryptionProviderFactory : IProviderFactory<IAsymmetricEncryptionProvider>
{
    IAsymmetricEncryptionProvider GetProviderForDecrypting(string value);
}
