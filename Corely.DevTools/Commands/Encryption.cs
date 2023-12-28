using Corely.Common.Providers.Security.Factories;
using Corely.Common.Providers.Security.Keys;
using Corely.DevTools.Attributes;

namespace Corely.DevTools.Commands
{
    internal class Encryption : CommandBase
    {
        [Argument("Key to validate. Also used for encryption and decryption.", false)]
        private string Key { get; init; }

        [Argument("Code for encryption type to use", false)]
        private string EncryptionTypeCode { get; init; }

        [Option("-c", "--create", Description = "Create a new key")]
        private bool Create { get; init; }

        [Option("-e", "--encrypt", Description = "Encrypt a value")]
        private string ToEncrypt { get; init; }

        [Option("-d", "--decrypt", Description = "Decrypt a value")]
        private string ToDecrypt { get; init; }

        public Encryption() : base("encrypt", "Encryption operations", "Default: List encryption types if no argument or option is provided")
        {
        }

        public override void Execute()
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
            var encryptionProviderFactor = new EncryptionProviderFactory(new InMemoryKeyStoreProvider(Key));
            var providers = encryptionProviderFactor.ListProviders();
            foreach (var (ProviderCode, ProviderType) in providers)
            {
                Console.WriteLine($"Code {ProviderCode} = {ProviderType.Name}");
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
            var encrypted = encryptionProviderFactor.GetProvider(EncryptionTypeCode)
                .Encrypt(ToEncrypt);
            Console.WriteLine(encrypted);
        }

        private void Decrypt()
        {
            var encryptionProviderFactor = new EncryptionProviderFactory(new InMemoryKeyStoreProvider(Key));
            var decrypted = encryptionProviderFactor.GetProvider(EncryptionTypeCode)
                .Decrypt(ToDecrypt);
            Console.WriteLine(decrypted);
        }
    }
}
