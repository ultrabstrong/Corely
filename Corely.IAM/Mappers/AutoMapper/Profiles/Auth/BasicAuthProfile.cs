using AutoMapper;
using Corely.IAM.Entities.Auth;
using Corely.IAM.Models.Auth;

namespace Corely.IAM.Mappers.AutoMapper.Profiles.Auth
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
