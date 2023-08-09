using Corely.Shared.Core.Extensions;

namespace Corely.Shared.Security
{
    public class AESEncryptionProvider : IEncryptionProvider
    {
        private readonly string _key;
        private readonly IKeyProvider _keyProvider;

        public AESEncryptionProvider(IKeyProvider keyProvider, string key)
        {
            _key = key;
            _keyProvider = keyProvider.ThrowIfNull();
        }

        public string Decrypt(string value)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string value)
        {
            throw new NotImplementedException();
        }
    }
}
