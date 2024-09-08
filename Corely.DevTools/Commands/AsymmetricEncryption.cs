﻿using Corely.DevTools.Attributes;
using Corely.Security.Encryption;
using Corely.Security.Encryption.Factories;
using Corely.Security.Keys;
using Corely.Security.KeyStore;

namespace Corely.DevTools.Commands
{
    internal class AsymmetricEncryption : CommandBase
    {
        private const string DEFAULT_ENCRYPTION_TYPE = AsymmetricEncryptionConstants.RSA_SHA256_CODE;

        [Argument("File with keys to validate (default), encrypt value (-e flag), or decrypt value (-d flag). Format public<newline>private", true)]
        private string KeyFile { get; init; } = null!;

        [Argument("Code for encryption type to use (hint: use -l to list codes. default used if code not provided)", false)]
        private string EncryptionTypeCode { get; init; } = null!;

        [Option("-l", "--list", Description = "List asymmetric encryption providers")]
        private bool List { get; init; }

        [Option("-c", "--create", Description = "Create a new asymmetric keys")]
        private bool Create { get; init; }

        [Option("-e", "--encrypt", Description = "Encrypt a value")]
        private string ToEncrypt { get; init; } = null!;

        [Option("-d", "--decrypt", Description = "Decrypt a value")]
        private string ToDecrypt { get; init; } = null!;

        [Option("-v", "--validate", Description = "Validate a key")]
        private bool Validate { get; init; }

        public AsymmetricEncryption() : base("asym-encrypt", "Asymmetric encryption operations", "Use at least one flag to perform an operation")
        {
        }

        protected override void Execute()
        {
            if (List)
            {
                ListProviders();
            }
            if (Create)
            {
                CreateKeys();
            }
            if (!string.IsNullOrEmpty(ToEncrypt))
            {
                Encrypt();
            }
            if (!string.IsNullOrEmpty(ToDecrypt))
            {
                Decrypt();
            }
            if (Validate)
            {
                ValidateKey();
            }

            if (!List
                && !Create
                && string.IsNullOrEmpty(ToEncrypt)
                && string.IsNullOrEmpty(ToDecrypt)
                && !Validate)
            {
                ShowHelp();
            }
        }

        private (string publicKey, string privateKey) ReadKeysFromFile()
        {
            var keys = File.ReadAllLines(KeyFile);
            if (keys.Length != 2)
            {
                throw new Exception("Invalid key file format. Must be public<newline>private");
            }

            return (keys[0], keys[1]);
        }

        private static void ListProviders()
        {
            var encryptionProviderFactory = new AsymmetricEncryptionProviderFactory(DEFAULT_ENCRYPTION_TYPE);
            var providers = encryptionProviderFactory.ListProviders();
            foreach (var (providerCode, providerType) in providers)
            {
                Console.WriteLine($"Code {providerCode} = {providerType.Name} {(providerCode == DEFAULT_ENCRYPTION_TYPE ? "(default)" : "")}");
            }
        }

        private void CreateKeys()
        {
            var (publicKey, privateKey) = new RsaKeyProvider().CreateKeys();
            File.WriteAllText(KeyFile, $"{publicKey}{Environment.NewLine}{privateKey}");
            Console.WriteLine($"Keys written to {KeyFile}");
        }

        private void ValidateKey()
        {
            var (publicKey, privateKey) = ReadKeysFromFile();
            var isValid = new RsaKeyProvider().IsKeyValid(publicKey, privateKey);
            Console.WriteLine($"Keys are {(isValid ? "valid" : "invalid")}");
        }

        private void Encrypt()
        {
            var encryptionProviderFactory = new AsymmetricEncryptionProviderFactory(DEFAULT_ENCRYPTION_TYPE);
            var (publicKey, privateKey) = ReadKeysFromFile();
            var keyProvider = new InMemoryAsymmetricKeyStoreProvider(publicKey, privateKey);
            var encrypted = encryptionProviderFactory.GetProvider(EncryptionTypeCode ?? DEFAULT_ENCRYPTION_TYPE)
                .Encrypt(ToEncrypt, keyProvider);
            Console.WriteLine(encrypted);
        }

        private void Decrypt()
        {
            var encryptionProviderFactory = new AsymmetricEncryptionProviderFactory(DEFAULT_ENCRYPTION_TYPE);
            var (publicKey, privateKey) = ReadKeysFromFile();
            var keyProvider = new InMemoryAsymmetricKeyStoreProvider(publicKey, privateKey);
            var decrypted = encryptionProviderFactory.GetProvider(EncryptionTypeCode ?? DEFAULT_ENCRYPTION_TYPE)
                .Decrypt(ToDecrypt, keyProvider);
            Console.WriteLine(decrypted);
        }
    }
}
