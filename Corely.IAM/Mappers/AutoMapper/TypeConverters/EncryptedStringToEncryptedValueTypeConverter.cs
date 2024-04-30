using AutoMapper;
using Corely.Security.Encryption.Factories;
using Corely.Security.Encryption.Models;

namespace Corely.IAM.Mappers.AutoMapper.TypeConverters
{
    internal sealed class EncryptedStringToEncryptedValueTypeConverter : ITypeConverter<string, ISymmetricEncryptedValue>
    {
        private readonly ISymmetricEncryptionProviderFactory _encryptionProviderFactory;

        public EncryptedStringToEncryptedValueTypeConverter(ISymmetricEncryptionProviderFactory encryptionProviderFactory)
        {
            _encryptionProviderFactory = encryptionProviderFactory;
        }

        public ISymmetricEncryptedValue Convert(string source, ISymmetricEncryptedValue destination, ResolutionContext context)
        {
            var encryptionProvider = _encryptionProviderFactory.GetProviderForDecrypting(source);
            return new SymmetricEncryptedValue(encryptionProvider) { Secret = source };
        }
    }
}
