namespace Corely.Domain.Entities.Users
{
    public class UserDetailsEntity : IHasCreatedUtc, IHasModifiedUtc
    {
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
    }
}
