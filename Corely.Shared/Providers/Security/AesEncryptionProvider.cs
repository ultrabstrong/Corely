using Corely.Shared.Extensions;
using System.Security.Cryptography;

namespace Corely.Shared.Providers.Security
{
    public sealed class AESEncryptionProvider : IEncryptionProvider
    {
        private readonly string _key;
        private readonly IKeyProvider _keyProvider;

        public AESEncryptionProvider(IKeyProvider keyProvider, ISecretProvider secretProvider)
        {
            _key = secretProvider.ThrowIfNull(nameof(secretProvider)).Get();
            _keyProvider = keyProvider.ThrowIfNull(nameof(keyProvider));
            if (!_keyProvider.IsKeyValid(_key))
            {
                throw new ArgumentException("Key is not valid");
            }
        }

        public string Encrypt(string value)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(value, nameof(value));

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

        public string Decrypt(string value)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(value, nameof(value));

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
