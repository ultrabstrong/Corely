using Corely.Shared.Attributes.Db;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Corely.Domain.Entities.Users
{
    [Table("Users")]
    public class UserEntity
    {
        [Key, AutoIncrement]
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedUtc { get; set; }

        public bool Enabled { get; set; }

        [MaxLength(50)]
        public string Username { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        public UserDetailsEntity? Details { get; set; }
    }
}
