using AutoMapper;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Models.Auth;

namespace Corely.Domain.Mappers.AutoMapper.Profiles.Auth
{
    internal sealed class BasicAuthProfile : Profile
    {
        public BasicAuthProfile()
        {
            CreateMap<BasicAuth, BasicAuthEntity>(MemberList.Source)
                .ReverseMap();
        }
    }
}
