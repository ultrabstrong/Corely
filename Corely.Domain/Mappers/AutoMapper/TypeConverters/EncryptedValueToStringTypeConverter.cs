using AutoMapper;
using Corely.Security.Encryption.Models;

namespace Corely.Domain.Mappers.AutoMapper.TypeConverters
{
    internal sealed class EncryptedValueToStringTypeConverter : ITypeConverter<IEncryptedValue, string?>
    {
        public string? Convert(IEncryptedValue source, string? _, ResolutionContext __)
        {
            return source?.Secret;
        }
    }
}
