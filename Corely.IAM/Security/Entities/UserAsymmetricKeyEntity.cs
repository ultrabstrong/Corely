using Corely.IAM.Entities;

namespace Corely.IAM.Security.Entities
{
    public class UserAsymmetricKeyEntity : IHasCreatedUtc, IHasModifiedUtc
    {
        public int UserId { get; set; }
        public int Version { get; set; }
        public string PublicKey { get; set; } = null!;
        public string PrivateKey { get; set; } = null!;
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
    }
}
