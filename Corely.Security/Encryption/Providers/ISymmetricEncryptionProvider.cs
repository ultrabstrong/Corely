﻿using Corely.Security.KeyStore;

namespace Corely.Security.Encryption.Providers
{
    public interface ISymmetricEncryptionProvider
    {
        string EncryptionTypeCode { get; }
        string Encrypt(string value, ISymmetricKeyStoreProvider keyStoreProvider);
        string Decrypt(string value, ISymmetricKeyStoreProvider keyStoreProvider);
        string ReEncrypt(string value, ISymmetricKeyStoreProvider keyStoreProvider);
        string? RemoveEncodedEncryptionData(string value);
    }
}
