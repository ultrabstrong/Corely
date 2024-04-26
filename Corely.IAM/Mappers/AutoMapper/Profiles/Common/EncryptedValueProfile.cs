using AutoMapper;
using Corely.IAM.Mappers.AutoMapper.TypeConverters;
using Corely.Security.Encryption.Models;

namespace Corely.IAM.Mappers.AutoMapper.Profiles.Common
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
