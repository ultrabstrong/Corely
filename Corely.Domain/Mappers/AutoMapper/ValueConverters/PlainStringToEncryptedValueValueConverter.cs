using AutoMapper;
using Corely.Security.Encryption.Factories;
using Corely.Security.Encryption.Models;

namespace Corely.Domain.Mappers.AutoMapper.ValueConverters
{
    internal sealed class PlainStringToEncryptedValueValueConverter : IValueConverter<string, IEncryptedValue>
    {
        private readonly IEncryptionProviderFactory _encryptionProviderFactory;

        public PlainStringToEncryptedValueValueConverter(IEncryptionProviderFactory encryptionProviderFactory)
        {
            _encryptionProviderFactory = encryptionProviderFactory;
        }

        public IEncryptedValue Convert(string source, ResolutionContext context)
        {
            var encryptionProvider = _encryptionProviderFactory.GetDefaultProvider();
            var encryptedValue = new EncryptedValue(encryptionProvider);
            encryptedValue.Set(source);
            return encryptedValue;
        }
    }
}
