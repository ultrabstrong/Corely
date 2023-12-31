using AutoMapper;
using Corely.Common.Models.Security;
using Corely.Common.Providers.Security.Factories;

namespace Corely.Domain.Mappers.AutoMapper.TypeConverters
{
    internal sealed class StringToEncryptedValueTypeConverter : ITypeConverter<string, IEncryptedValue>
    {
        private readonly IEncryptionProviderFactory _encryptionProviderFactory;

        public StringToEncryptedValueTypeConverter(IEncryptionProviderFactory encryptionProviderFactory)
        {
            _encryptionProviderFactory = encryptionProviderFactory;
        }

        public IEncryptedValue Convert(string source, IEncryptedValue destination, ResolutionContext context)
        {
            var encryptionProvider = _encryptionProviderFactory.GetProviderForDecrypting(source);
            return new EncryptedValue(encryptionProvider) { Secret = source };
        }
    }
}
