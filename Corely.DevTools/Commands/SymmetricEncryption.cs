using Corely.DevTools.Attributes;
using Corely.Security.Encryption.Factories;
using Corely.Security.Keys;
using Corely.Security.KeyStore;

namespace Corely.DevTools.Commands
{
    internal class SymmetricEncryption : CommandBase
    {
        [Argument("Key to validate. Also used for encryption and decryption.", false)]
        private string Key { get; init; } = null!;

        [Argument("Code for encryption type to use", false)]
        private string EncryptionTypeCode { get; init; } = null!;

        [Option("-c", "--create", Description = "Create a new symmetric key")]
        private bool Create { get; init; }

        [Option("-e", "--encrypt", Description = "Encrypt a value")]
        private string ToEncrypt { get; init; } = null!;

        [Option("-d", "--decrypt", Description = "Decrypt a value")]
        private string ToDecrypt { get; init; } = null!;

        public SymmetricEncryption() : base("sym-encrypt", "Symmetric encryption operations", "Default: List symmetric encryption types if no argument or option is provided")
        {
        }

        protected override void Execute()
        {
            if (Create)
            {
                CreateKey();
            }
            else if (string.IsNullOrWhiteSpace(Key))
            {
                ListProviders();
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

        private void ListProviders()
        {
            var encryptionProviderFactory = new SymmetricEncryptionProviderFactory(EncryptionTypeCode);
            var providers = encryptionProviderFactory.ListProviders();
            foreach (var (ProviderCode, ProviderType) in providers)
            {
                Console.WriteLine($"Code {ProviderCode} = {ProviderType.Name}");
            }
        }

        private static void CreateKey()
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
            var encryptionProviderFactory = new SymmetricEncryptionProviderFactory(EncryptionTypeCode);
            var keyProvider = new InMemorySymmetricKeyStoreProvider(Key);
            var encrypted = encryptionProviderFactory.GetProvider(EncryptionTypeCode)
                .Encrypt(ToEncrypt, keyProvider);
            Console.WriteLine(encrypted);
        }

        private void Decrypt()
        {
            var encryptionProviderFactory = new SymmetricEncryptionProviderFactory(EncryptionTypeCode);
            var keyProvider = new InMemorySymmetricKeyStoreProvider(Key);
            var decrypted = encryptionProviderFactory.GetProvider(EncryptionTypeCode)
                .Decrypt(ToDecrypt, keyProvider);
            Console.WriteLine(decrypted);
        }
    }
}
