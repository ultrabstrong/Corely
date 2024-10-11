using Corely.IAM.Entities;
using Corely.IAM.Security.Entities;

namespace Corely.IAM.Users.Entities
{
    public class UserAsymmetricKeyEntity : AsymmetricKeyEntity, IHasIdPk
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}
