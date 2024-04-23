using AutoMapper;
using Corely.Security.Hashing.Factories;
using Corely.Security.Hashing.Models;

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
