using AutoMapper;
using Corely.Common.Models.Security;

namespace Corely.Domain.Mappers.AutoMapper.TypeConverters
{
    public sealed class HashedValueToStringTypeConverter : ITypeConverter<IHashedValue, string?>
    {
        public string? Convert(IHashedValue source, string? _, ResolutionContext __)
        {
            return source?.Hash;
        }
    }
}
