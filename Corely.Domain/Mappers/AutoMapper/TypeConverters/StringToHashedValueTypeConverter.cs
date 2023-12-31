using AutoMapper;
using Corely.Common.Models.Security;
using Corely.Common.Providers.Security.Factories;

namespace Corely.Domain.Mappers.AutoMapper.TypeConverters
{
    public class StringToHashedValueTypeConverter(
        IHashProviderFactory hashProviderFactory)
        : ITypeConverter<string, IHashedValue>
    {
        private readonly IHashProviderFactory _hashProviderFactory = hashProviderFactory;

        public IHashedValue Convert(string source, IHashedValue destination, ResolutionContext context)
        {
            var hashProvider = _hashProviderFactory.GetProviderToVerify(source);
            return new HashedValue(hashProvider) { Hash = source };
        }
    }
}
