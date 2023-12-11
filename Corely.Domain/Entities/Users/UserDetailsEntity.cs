namespace Corely.Domain.Entities.Users
{
    public class UserDetailsEntity
    {
        public UserEntity User { get; set; }

        public int UserId { get; set; }

        public string? Name { get; set; }
        public string? Phone { get; set; }

        public string? Address { get; set; }

        public byte[]? ProfilePicture { get; set; }


    }
}
