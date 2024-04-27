using AutoMapper;
using Corely.IAM.Users.Entities;
using Corely.IAM.Users.Models;

namespace Corely.IAM.Mappers.AutoMapper.UserProfiles
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
