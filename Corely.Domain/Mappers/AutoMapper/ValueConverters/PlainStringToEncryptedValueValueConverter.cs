using AutoMapper;
using Corely.Common.Models.Security;
using Corely.Common.Providers.Security.Factories;

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
