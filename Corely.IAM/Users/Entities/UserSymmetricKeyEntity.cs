using Corely.IAM.Security.Entities;

namespace Corely.IAM.Users.Entities
{
    public class UserSymmetricKeyEntity : SymmetricKeyEntity
    {
        public int UserId { get; set; }
    }
}
