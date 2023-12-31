using AutoMapper;
using Corely.Common.Models.Security;

namespace Corely.Domain.Mappers.AutoMapper.TypeConverters
{
    public sealed class EncryptedValueToStringTypeConverter : ITypeConverter<IEncryptedValue, string?>
    {
        public string? Convert(IEncryptedValue source, string? _, ResolutionContext __)
        {
            return source?.Secret;
        }
    }
}
