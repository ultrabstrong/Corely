using Corely.IAM.Security.Entities;

namespace Corely.IAM.Users.Entities
{
    public class UserAsymmetricKeyEntity : AsymmetricKeyEntity
    {
        public int UserId { get; set; }
    }
}
