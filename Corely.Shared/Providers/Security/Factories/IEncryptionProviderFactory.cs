using Corely.Shared.Providers.Security.Encryption;

namespace Corely.Shared.Providers.Security.Factories
{
    public interface IEncryptionProviderFactory
    {
        IEncryptionProvider Create();

        IEncryptionProvider CreateForDecrypting(string value);
    }
}
