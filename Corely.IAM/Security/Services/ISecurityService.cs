using Corely.IAM.Security.Models;

namespace Corely.IAM.Security.Services
{
    public interface ISecurityService
    {
        SymmetricKey GetSymmetricKeyEncryptedWithSystemKey();
        AsymmetricKey GetAsymmetricKeyEncryptedWithSystemKey();
    }
}
