using Corely.Shared.Providers.Security;
using Corely.Shared.Models.Security;

namespace Corely.IAM.Security.Models;

public class SymmetricEncryptedValue : EncryptedValue, ISymmetricEncryptedValue
{
    public SymmetricEncryptedValue(IEncryptionProvider encryptionProvider) 
        : base(encryptionProvider)
    {
    }

    public SymmetricEncryptedValue(IEncryptionProvider encryptionProvider, string secret) 
        : base(encryptionProvider, secret)
    {
    }
}