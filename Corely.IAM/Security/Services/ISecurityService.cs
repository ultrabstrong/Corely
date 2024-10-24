﻿using Corely.IAM.Security.Models;
using Microsoft.IdentityModel.Tokens;

namespace Corely.IAM.Security.Services
{
    public interface ISecurityService
    {
        SymmetricKey GetSymmetricEncryptionKeyEncryptedWithSystemKey();
        AsymmetricKey GetAsymmetricEncryptionKeyEncryptedWithSystemKey();
        AsymmetricKey GetAsymmetricSignatureKeyEncryptedWithSystemKey();
        string DecryptWithSystemKey(string encryptedValue);
        SigningCredentials GetAsymmetricSigningCredentials(string providerTypeCode, string privateKey);
    }
}
