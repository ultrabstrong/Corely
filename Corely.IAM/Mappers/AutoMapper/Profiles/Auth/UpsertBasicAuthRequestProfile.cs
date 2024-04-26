using AutoMapper;
using Corely.IAM.Auth.Models;
using Corely.IAM.Mappers.AutoMapper.ValueConverters;

namespace Corely.IAM.Mappers.AutoMapper.Profiles.Auth
{
    internal class UpsertBasicAuthRequestProfile : Profile
    {
        public UpsertBasicAuthRequestProfile()
        {
            CreateMap<UpsertBasicAuthRequest, BasicAuth>(MemberList.Source)
                .ForMember(dest => dest.Password,
                    opt => opt.ConvertUsing<PlainStringToHashedValueValueConverter, string>());
        }
    }
}
