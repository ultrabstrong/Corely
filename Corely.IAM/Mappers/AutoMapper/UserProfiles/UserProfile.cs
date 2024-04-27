using AutoMapper;
using Corely.IAM.Users.Entities;
using Corely.IAM.Users.Models;

namespace Corely.IAM.Mappers.AutoMapper.UserProfiles
{
    internal sealed class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserEntity>(MemberList.Source)
                .ReverseMap();
        }
    }
}
