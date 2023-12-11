using AutoMapper;
using Corely.Domain.Entities.Users;
using Corely.Domain.Models.Auth;

namespace Corely.Domain.Models.Users
{
    public class User : Profile
    {
        public int Id { get; private set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public UserDetails? Details { get; set; }
        public BasicAuth? BasicAuth { get; set; }
        public User(int id)
        {
            Id = id;
            CreateMap<User, UserEntity>();
        }
    }
}
