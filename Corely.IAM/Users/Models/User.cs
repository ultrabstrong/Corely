namespace Corely.IAM.Users.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool Enabled { get; set; }
        public int SuccessfulLogins { get; set; }
        public int FailedLogins { get; set; }
        public int FailedLoginsSinceLastSuccess { get; set; }
        public DateTime? LastFailedLoginUtc { get; set; }
        public DateTime? LastLoginUtc { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
        public UserDetails? Details { get; init; }
    }
}
