namespace Corely.IAM.Users.Models
{
    public class User
    {
        public int Id { get; init; }
        public string Username { get; init; } = null!;
        public string Email { get; init; } = null!;
        public bool Enabled { get; init; }
        public DateTime CreatedUtc { get; init; }
        public UserDetails? Details { get; init; }
    }
}
