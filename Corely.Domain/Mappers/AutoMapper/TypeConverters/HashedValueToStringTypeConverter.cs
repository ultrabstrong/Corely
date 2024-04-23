using AutoMapper;
using Corely.Security.Hashing.Models;

namespace Corely.Domain.Mappers.AutoMapper.TypeConverters
{
    internal sealed class HashedValueToStringTypeConverter : ITypeConverter<IHashedValue, string?>
    {
        public string? Convert(IHashedValue source, string? _, ResolutionContext __)
        {
            return source?.Hash;
        }
    }
}
