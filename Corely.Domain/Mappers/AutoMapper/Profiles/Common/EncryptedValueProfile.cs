using AutoMapper;
using Corely.Common.Models.Security;
using Corely.Domain.Mappers.AutoMapper.TypeConverters;

namespace Corely.Domain.Mappers.AutoMapper.Profiles.Common
{
    public class EncryptedValueProfile : Profile
    {
        public EncryptedValueProfile()
        {
            CreateMap<IEncryptedValue, string?>().ConvertUsing<EncryptedValueToStringTypeConverter>();
            CreateMap<string, IEncryptedValue>().ConvertUsing<StringToEncryptedValueTypeConverter>();
        }
    }
}
