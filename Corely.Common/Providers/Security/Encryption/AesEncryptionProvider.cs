using Corely.Common.Providers.Security.Keys;
using System.Security.Cryptography;

namespace Corely.Common.Providers.Security.Encryption
{
    internal sealed class AesEncryptionProvider : EncryptionProviderBase
    {
        public override string EncryptionTypeCode => EncryptionProviderConstants.AES_CODE;

        public AesEncryptionProvider(IKeyStoreProvider keyStoreProvider)
            : base(keyStoreProvider) { }
        protected override string EncryptInternal(string value, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(key);
                aes.GenerateIV();

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (MemoryStream msEncrypt = new())
                {
                    // Prepend IV to the beginning of the encrypted string
                    msEncrypt.Write(aes.IV, 0, aes.IV.Length);

                    using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new(csEncrypt))
                    {
                        swEncrypt.Write(value);
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        protected override string DecryptInternal(string value, string key)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] fullCipher = Convert.FromBase64String(value);

                byte[] iv = new byte[aes.IV.Length];
                byte[] cipherText = new byte[fullCipher.Length - iv.Length];

                // Extract IV from the beginning of the encrypted string
                Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(fullCipher, iv.Length, cipherText, 0, cipherText.Length);

                aes.Key = Convert.FromBase64String(key);
                aes.IV = iv;

                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (MemoryStream msDecrypt = new(cipherText))
                using (CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
    }
}
