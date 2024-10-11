using Corely.IAM.Entities;
using Corely.IAM.Security.Enums;

namespace Corely.IAM.Security.Entities
{
    public class SymmetricKeyEntity : IHasCreatedUtc, IHasModifiedUtc
    {
        public KeyUsedFor KeyUsedFor { get; set; }
        public int Version { get; set; }
        public string Key { get; set; } = null!;
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
    }
}
