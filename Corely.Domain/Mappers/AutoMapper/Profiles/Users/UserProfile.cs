using AutoMapper;
using Corely.Domain.Entities.Users;
using Corely.Domain.Models.Users;

namespace Corely.Domain.Mappers.AutoMapper.Profiles.Users
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
