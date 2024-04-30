using Corely.Security.Encryption.Models;

namespace Corely.IAM.Security.Models
{
    public class SymmetricKey
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public ISymmetricEncryptedValue Key { get; set; } = null!;
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
    }
}
