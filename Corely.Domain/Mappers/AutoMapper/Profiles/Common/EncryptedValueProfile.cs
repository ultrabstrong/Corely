using AutoMapper;
using Corely.Domain.Mappers.AutoMapper.TypeConverters;
using Corely.Security.Encryption.Models;

namespace Corely.Domain.Mappers.AutoMapper.Profiles.Common
{
    internal class EncryptedValueProfile : Profile
    {
        public EncryptedValueProfile()
        {
            CreateMap<IEncryptedValue, string?>().ConvertUsing<EncryptedValueToStringTypeConverter>();
            CreateMap<string, IEncryptedValue>().ConvertUsing<EncryptedStringToEncryptedValueTypeConverter>();
        }
    }
}
