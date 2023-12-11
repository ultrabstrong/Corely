using AutoMapper;
using Corely.Domain.Entities.Users;

namespace Corely.Domain.Models.Users
{
    public class UserDetails : Profile
    {
        public string? Name { get; init; }
        public string? Phone { get; init; }
        public string? Address { get; init; }
        public byte[]? ProfilePicture { get; init; }

        public UserDetails()
        {
            CreateMap<UserDetails, UserDetailsEntity>();
        }
    }
}
