using AutoMapper;
using Corely.Domain.Entities.Users;
using Corely.Domain.Models.Users;

namespace Corely.Domain.Mappers.AutoMapper.Profiles.Users
{
    public sealed class UserDetailsProfile : Profile
    {
        public UserDetailsProfile()
        {
            CreateMap<UserDetails, UserDetailsEntity>()
                .ReverseMap();
        }
    }
}
