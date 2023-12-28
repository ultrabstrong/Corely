using Corely.Common.Providers.Security.Encryption;
using Corely.Common.Providers.Security.Factories;
using Corely.Common.Providers.Security.Keys;
using Corely.DevTools.Attributes;

namespace Corely.DevTools.Commands
{
    internal class Aes : CommandBase
    {
        [Argument("Key to validate. Also used for encryption and decryption.", false)]
        private string Key { get; init; }

        [Option("-e", "--encrypt", Description = "Encrypt a value")]
        private string ToEncrypt { get; init; }

        [Option("-d", "--decrypt", Description = "Decrypt a value")]
        private string ToDecrypt { get; init; }

        public Aes() : base("aes", "Aes encryption operations", "Default : Create new key if no argument or option is provided")
        {
        }

        public override void Execute()
        {
            if (string.IsNullOrWhiteSpace(Key))
            {
                CreateKey();
            }
            else
            {
                if (!string.IsNullOrEmpty(ToEncrypt))
                {
                    Encrypt();
                }
                if (!string.IsNullOrEmpty(ToDecrypt))
                {
                    Decrypt();
                }
                if (string.IsNullOrEmpty(ToEncrypt) && string.IsNullOrEmpty(ToDecrypt))
                {
                    ValidateKey();
                }
            }
        }

        private void CreateKey()
        {
            var key = new AesKeyProvider().CreateKey();
            Console.WriteLine(key);
        }

        private void ValidateKey()
        {
            var isValid = new AesKeyProvider().IsKeyValid(Key);
            Console.WriteLine($"Key is {(isValid ? "valid" : "invalid")}");
        }

        private void Encrypt()
        {
            var encryptionProviderFactor = new EncryptionProviderFactory(new InMemoryKeyStoreProvider(Key));
            var encrypted = encryptionProviderFactor.GetProvider(EncryptionProviderConstants.AES)
                .Encrypt(ToEncrypt);
            Console.WriteLine(encrypted);
        }

        private void Decrypt()
        {
            var encryptionProviderFactor = new EncryptionProviderFactory(new InMemoryKeyStoreProvider(Key));
            var decrypted = encryptionProviderFactor.GetProvider(EncryptionProviderConstants.AES)
                .Decrypt(ToDecrypt);
            Console.WriteLine(decrypted);
        }
    }
}
