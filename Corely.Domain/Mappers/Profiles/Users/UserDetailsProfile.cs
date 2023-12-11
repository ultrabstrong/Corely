using AutoMapper;
using Corely.Domain.Entities.Users;
using Corely.Domain.Models.Users;

namespace Corely.Domain.Mappers.Profiles.Users
{
    public class UserDetailsProfile : Profile
    {
        public UserDetailsProfile()
        {
            CreateMap<UserDetails, UserDetailsEntity>();
        }
    }
}
