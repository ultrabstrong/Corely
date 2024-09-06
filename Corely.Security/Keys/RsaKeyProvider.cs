using System.Security.Cryptography;

namespace Corely.Security.Keys
{
    public sealed class RsaKeyProvider : IAsymmetricKeyProvider
    {
        public const int DEFAULT_KEY_SIZE = 2048;

        private readonly int _keySize;

        public RsaKeyProvider(int keySize = DEFAULT_KEY_SIZE)
        {
            if (keySize < 0 || keySize % 8 != 0)
            {
                throw new ArgumentException("Key size must be a positive multiple of 8.", nameof(keySize));
            }

            _keySize = keySize;
        }

        public (string PublicKey, string PrivateKey) CreateKeyPair()
        {
            using (var rsa = new RSACryptoServiceProvider(_keySize))
            {
                try
                {
                    rsa.PersistKeyInCsp = false;
                    var publicKey = GetPublicKey(rsa);
                    var privateKey = GetPrivateKey(rsa);
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
                    var testPublicKey = GetPublicKey(rsa);

                    rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
                    var testPrivateKey = GetPrivateKey(rsa);


                    return testPublicKey == publicKey &&
                           testPrivateKey == privateKey;
                }
            }
            catch
            {
                return false;
            }
        }

        private static string GetPrivateKey(RSACryptoServiceProvider rsa)
        {
            return Convert.ToBase64String(rsa.ExportPkcs8PrivateKey());
        }

        private static string GetPublicKey(RSACryptoServiceProvider rsa)
        {
            return Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo());
        }
    }
}
