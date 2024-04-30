using AutoMapper;
using Corely.Security.Encryption.Models;

namespace Corely.IAM.Mappers.AutoMapper.TypeConverters
{
    internal sealed class EncryptedValueToStringTypeConverter : ITypeConverter<ISymmetricEncryptedValue, string?>
    {
        public string? Convert(ISymmetricEncryptedValue source, string? _, ResolutionContext __)
        {
            return source?.Secret;
        }
    }
}
