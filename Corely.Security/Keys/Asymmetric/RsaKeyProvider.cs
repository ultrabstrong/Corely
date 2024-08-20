using System.Security.Cryptography;

namespace Corely.Security.Keys.Asymmetric
{
    public sealed class RsaKeyProvider : IAsymmetricKeyProvider
    {
        public const int DEFAULT_KEY_SIZE = 2048;

        private readonly int _keySize;
        private readonly HashAlgorithmName _hashAlgorithm;

        public RsaKeyProvider(
            int keySize = DEFAULT_KEY_SIZE, 
            HashAlgorithmName hashAlgorithm = default)
        {
            if (keySize < 0 || keySize % 8 != 0)
            {
                throw new ArgumentException("Key size must be a positive multiple of 8.", nameof(keySize));
            }

            _keySize = keySize;
            _hashAlgorithm = hashAlgorithm == default 
                ? HashAlgorithmName.SHA256 
                : hashAlgorithm;
        }

        public (string PublicKey, string PrivateKey) CreateKeyPair()
        {
            using (var rsa = new RSACryptoServiceProvider(_keySize))
            {
                try
                {
                    rsa.PersistKeyInCsp = false;
                    var publicKey = Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo());
                    var privateKey = Convert.ToBase64String(rsa.ExportPkcs8PrivateKey());
                    return (publicKey, privateKey);
                }
                finally
                {
                    rsa.Clear();
                }
            }
        }

        public bool IsKeyValid(string publicKey, string privateKey)
        {
            try
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    var publicKeyBytes = Convert.FromBase64String(publicKey);
                    var privateKeyBytes = Convert.FromBase64String(privateKey);

                    rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
                    var testPublicKey = rsa.ExportSubjectPublicKeyInfo();

                    rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
                    var testPrivateKey = rsa.ExportPkcs8PrivateKey();

                    return Convert.ToBase64String(testPublicKey) == publicKey &&
                           Convert.ToBase64String(testPrivateKey) == privateKey;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
