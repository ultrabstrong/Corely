using Corely.IAM.Security.Models;

namespace Corely.IAM.Accounts.Models
{
    public class Account
    {
        public int Id { get; init; }
        public string AccountName { get; init; } = null!;
        public SymmetricKey SymmetricKey { get; set; } = null!;
        public AsymmetricKey AsymmetricKey { get; set; } = null!;
    }
}
