using AutoMapper;
using Corely.IAM.Entities.Users;
using Corely.IAM.Models.Users;

namespace Corely.IAM.Mappers.AutoMapper.Profiles.Users
{
    internal sealed class UserDetailsProfile : Profile
    {
        public UserDetailsProfile()
        {
            CreateMap<UserDetails, UserDetailsEntity>(MemberList.Source)
                .ReverseMap();
        }
    }
}
