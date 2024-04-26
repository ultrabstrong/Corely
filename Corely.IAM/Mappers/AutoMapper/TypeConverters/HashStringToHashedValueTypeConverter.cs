using AutoMapper;
using Corely.Security.Hashing.Factories;
using Corely.Security.Hashing.Models;

namespace Corely.IAM.Mappers.AutoMapper.TypeConverters
{
    internal sealed class HashStringToHashedValueTypeConverter : ITypeConverter<string, IHashedValue>
    {
        private readonly IHashProviderFactory _hashProviderFactory;

        public HashStringToHashedValueTypeConverter(IHashProviderFactory hashProviderFactory)
        {
            _hashProviderFactory = hashProviderFactory;
        }

        public IHashedValue Convert(string source, IHashedValue destination, ResolutionContext context)
        {
            var hashProvider = _hashProviderFactory.GetProviderToVerify(source);
            return new HashedValue(hashProvider) { Hash = source };
        }
    }
}
