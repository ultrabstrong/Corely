namespace Corely.Domain.Models.Users
{
    public class User
    {
        public int Id { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
        public bool Enabled { get; init; }
        public DateTime CreatedUtc { get; init; }
        public UserDetails? Details { get; init; }
    }
}
