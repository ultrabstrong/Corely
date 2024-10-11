using Corely.IAM.Security.Models;

namespace Corely.IAM.Security.Services
{
    public interface ISecurityService
    {
        SymmetricKey GetSymmetricEncryptionKeyEncryptedWithSystemKey();
        AsymmetricKey GetAsymmetricEncryptionKeyEncryptedWithSystemKey();
        AsymmetricKey GetAsymmetricSignatureKeyEncryptedWithSystemKey();
    }
}
