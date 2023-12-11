using AutoMapper;
using Corely.Domain.Entities.Users;

namespace Corely.Domain.Models.Users
{
    public class UserDetails : Profile
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public byte[]? ProfilePicture { get; set; }

        public UserDetails()
        {
            CreateMap<UserDetails, UserDetailsEntity>();
        }
    }
}
