using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Corely.Domain.Entities.Users
{
    [Table("UserDetails")]
    public class UserDetailsEntity
    {
        [Key]
        [ForeignKey(nameof(UserEntity.Id))]
        public int UserId { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }
    }
}
