using AutoMapper;
using Corely.Common.Models.Security;
using Corely.Common.Providers.Security.Factories;

namespace Corely.Domain.Mappers.AutoMapper.ValueConverters
{
    internal sealed class PlainStringToHashedValueValueConverter : IValueConverter<string, IHashedValue>
    {
        private readonly IHashProviderFactory _hashProviderFactory;

        public PlainStringToHashedValueValueConverter(IHashProviderFactory hashProviderFactory)
        {
            _hashProviderFactory = hashProviderFactory;
        }

        public IHashedValue Convert(string source, ResolutionContext context)
        {
            var hashProvider = _hashProviderFactory.GetDefaultProvider();
            var hashedValue = new HashedValue(hashProvider);
            hashedValue.Set(source);
            return hashedValue;
        }
    }
}
