using AutoMapper;
using Corely.IAM.BasicAuths.Entities;
using Corely.IAM.BasicAuths.Models;

namespace Corely.IAM.Mappers.AutoMapper.AuthProfiles
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
