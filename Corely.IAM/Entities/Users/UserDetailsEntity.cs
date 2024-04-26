namespace Corely.IAM.Entities.Users
{
    public class UserDetailsEntity : IHasCreatedUtc, IHasModifiedUtc
    {
        public int UserId { get; set; }
        public UserEntity User { get; set; } = null!;
        public string? Name { get; set; }
        public string? Phone { get; set; } = null!;
        public string? Address { get; set; } = null!;
        public byte[]? ProfilePicture { get; set; } = null!;
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
    }
}
