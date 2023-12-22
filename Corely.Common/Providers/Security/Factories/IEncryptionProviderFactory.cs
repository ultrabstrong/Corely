using Corely.Common.Providers.Security.Encryption;

namespace Corely.Common.Providers.Security.Factories
{
    public interface IEncryptionProviderFactory
    {
        IEncryptionProvider Create();

        IEncryptionProvider CreateForDecrypting(string value);
    }
}
