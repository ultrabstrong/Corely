using System.Security.Cryptography;

namespace Corely.Shared.Providers.Security
{
    public sealed class AESEncryptionProvider : EncryptionProviderBase
    {
        protected override string TwoDigitEncryptionTypeCode => EncryptionProviderCodeConstants.AES;

        public AESEncryptionProvider(ISecretProvider secretProvider)
            : base(secretProvider) { }

        protected override string EncryptInternal(string value)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(_key);
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

        protected override string DecryptInternal(string value)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] fullCipher = Convert.FromBase64String(value);

                byte[] iv = new byte[aes.IV.Length];
                byte[] cipherText = new byte[fullCipher.Length - iv.Length];

                // Extract IV from the beginning of the encrypted string
                Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(fullCipher, iv.Length, cipherText, 0, cipherText.Length);

                aes.Key = Convert.FromBase64String(_key);
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
