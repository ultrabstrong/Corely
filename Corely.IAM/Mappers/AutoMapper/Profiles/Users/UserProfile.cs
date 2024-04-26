using AutoMapper;
using Corely.IAM.Entities.Users;
using Corely.IAM.Models.Users;

namespace Corely.IAM.Mappers.AutoMapper.Profiles.Users
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
