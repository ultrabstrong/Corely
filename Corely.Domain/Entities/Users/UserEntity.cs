using System.ComponentModel.DataAnnotations;

namespace Corely.Domain.Entities.Users
{
    public class UserEntity
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedUtc { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        public UserDetailsEntity? Details { get; set; }
    }
}
