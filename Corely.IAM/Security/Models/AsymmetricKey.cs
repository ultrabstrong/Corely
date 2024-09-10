using Corely.Security.Encryption.Models;

namespace Corely.IAM.Security.Models
{
    public class AsymmetricKey
    {
        public int Version { get; set; }
        public string PublicKey { get; set; } = null!;
        public ISymmetricEncryptedValue PrivateKey { get; set; } = null!;
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
    }
}
