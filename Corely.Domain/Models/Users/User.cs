namespace Corely.Domain.Models.Users
{
    internal class User
    {
        public int Id { get; private set; }

        public DateTime CreatedUtc { get; set; }

        public bool Enabled { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public UserDetails Details { get; set; }

        public User(int id)
        {
            Id = id;
        }
    }
}
