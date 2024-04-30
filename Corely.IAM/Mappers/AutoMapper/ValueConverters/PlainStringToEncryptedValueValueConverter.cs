using AutoMapper;
using Corely.Security.Encryption.Factories;
using Corely.Security.Encryption.Models;

namespace Corely.IAM.Mappers.AutoMapper.ValueConverters
{
    internal sealed class PlainStringToEncryptedValueValueConverter : IValueConverter<string, ISymmetricEncryptedValue>
    {
        private readonly ISymmetricEncryptionProviderFactory _encryptionProviderFactory;

        public PlainStringToEncryptedValueValueConverter(ISymmetricEncryptionProviderFactory encryptionProviderFactory)
        {
            _encryptionProviderFactory = encryptionProviderFactory;
        }

        public ISymmetricEncryptedValue Convert(string source, ResolutionContext context)
        {
            var encryptionProvider = _encryptionProviderFactory.GetDefaultProvider();
            var encryptedValue = new SymmetricEncryptedValue(encryptionProvider);
            encryptedValue.Set(source);
            return encryptedValue;
        }
    }
}
