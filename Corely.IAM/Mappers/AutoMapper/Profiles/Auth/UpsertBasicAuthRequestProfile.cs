using AutoMapper;
using Corely.IAM.Mappers.AutoMapper.ValueConverters;
using Corely.IAM.Models.Auth;

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
