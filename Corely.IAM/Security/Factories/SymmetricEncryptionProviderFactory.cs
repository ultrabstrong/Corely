using Corely.Shared.Providers.Security;

namespace Corely.IAM.Security.Factories;

public class SymmetricEncryptionProviderFactory : ISymmetricEncryptionProviderFactory
{
    private readonly IEncryptionProvider _defaultProvider;

    public SymmetricEncryptionProviderFactory(IEncryptionProvider defaultProvider)
    {
        _defaultProvider = defaultProvider;
    }

    public IEncryptionProvider GetProviderForDecrypting(string encryptedValue)
    {
        // In a real implementation, this would examine the encrypted value
        // to determine which provider was used for encryption
        // For now, just return the default provider
        return _defaultProvider;
    }

    public IEncryptionProvider GetDefaultProvider()
    {
        return _defaultProvider;
    }
}