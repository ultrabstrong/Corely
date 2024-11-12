using Corely.DataAccess.Interfaces.Entities;
using Corely.IAM.Security.Entities;

namespace Corely.IAM.Users.Entities
{
    internal class UserAsymmetricKeyEntity : AsymmetricKeyEntity, IHasIdPk
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}
