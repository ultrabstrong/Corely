using AutoMapper;
using Corely.Domain.Entities.Users;
using Corely.Domain.Models.Users;

namespace Corely.Domain.Mappers.AutoMapper.Profiles.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserEntity>();
        }
    }
}
