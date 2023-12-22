using Corely.Common.Providers.Security.Encryption;

namespace Corely.Common.Providers.Security.Factories
{
    public interface IEncryptionProviderFactory
    {
        IEncryptionProvider Create(string providerCode);

        IEncryptionProvider CreateForDecrypting(string value);
    }
}
