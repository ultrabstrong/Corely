using System.ComponentModel.DataAnnotations;

namespace Corely.Domain.Entities.Auth
{
    public class BasicAuthEntity
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
