using AutoMapper;
using Corely.Domain.Mappers.AutoMapper.ValueConverters;
using Corely.Domain.Models.Auth;

namespace Corely.Domain.Mappers.AutoMapper.Profiles.Auth
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
