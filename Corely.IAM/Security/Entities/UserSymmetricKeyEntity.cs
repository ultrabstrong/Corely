using Corely.IAM.Entities;

namespace Corely.IAM.Security.Entities
{
    public class UserSymmetricKeyEntity : IHasCreatedUtc, IHasModifiedUtc
    {
        public int UserId { get; set; }
        public int Version { get; set; }
        public string Key { get; set; } = null!;
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
    }
}
