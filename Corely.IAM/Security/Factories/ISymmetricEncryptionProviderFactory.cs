using Corely.Shared.Providers.Security;

namespace Corely.IAM.Security.Factories;

public interface ISymmetricEncryptionProviderFactory
{
    IEncryptionProvider GetProviderForDecrypting(string encryptedValue);
    IEncryptionProvider GetDefaultProvider();
}