using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Corely.Domain.Entities.Users
{
    [Table("Users")]
    public class UserEntity
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedUtc { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [ForeignKey(nameof(UserDetailsEntity.UserId))]
        public UserDetailsEntity? Details { get; set; }
    }
}
