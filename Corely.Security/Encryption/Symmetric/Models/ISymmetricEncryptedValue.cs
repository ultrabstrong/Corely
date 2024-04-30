﻿using Corely.Security.KeyStore.Symmetric;

namespace Corely.Security.Encryption.Models
{
    public interface ISymmetricEncryptedValue
    {
        string Secret { get; }
        void Set(string decryptedValue, ISymmetricKeyStoreProvider provider);
        string GetDecrypted(ISymmetricKeyStoreProvider provider);
        void ReEncrypt(ISymmetricKeyStoreProvider provider);
    }
}
