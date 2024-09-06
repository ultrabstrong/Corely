﻿using Corely.Security.KeyStore;

namespace Corely.Security.Encryption.Providers
{
    public interface IAsymmetricEncryptionProvider
    {
        string EncryptionTypeCode { get; }
        string Encrypt(string value, IAsymmetricKeyStoreProvider keyStoreProvider);
        string Decrypt(string value, IAsymmetricKeyStoreProvider keyStoreProvider);
        string ReEncrypt(string value, IAsymmetricKeyStoreProvider keyStoreProvider);
    }
}