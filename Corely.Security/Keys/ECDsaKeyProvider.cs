using System.Security.Cryptography;

namespace Corely.Security.Keys
{
    internal sealed class EcdsaKeyProvider : IAsymmetricKeyProvider
    {
        public readonly ECCurve _ecCurve;

        public EcdsaKeyProvider()
        {
            _ecCurve = ECCurve.NamedCurves.nistP256;
        }

        public EcdsaKeyProvider(ECCurve curveType)
        {
            _ecCurve = curveType;
        }

        public (string PublicKey, string PrivateKey) CreateKeys()
        {
            using (var ecdsa = ECDsa.Create(_ecCurve))
            {
                var publicKey = GetPublicKey(ecdsa);
                var privateKey = GetPrivateKey(ecdsa);
                return (publicKey, privateKey);
            }
        }

        public bool IsKeyValid(string publicKey, string privateKey)
        {
            try
            {
                var publicKeyBytes = Convert.FromBase64String(publicKey);
                var privateKeyBytes = Convert.FromBase64String(privateKey);

                using var ecdsa = ECDsa.Create();

                ecdsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
                var testPrivateKey = GetPrivateKey(ecdsa);

                ecdsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
                var testPublicKey = GetPublicKey(ecdsa);


                return testPublicKey == publicKey && testPrivateKey == privateKey;
            }
            catch
            {
                return false;
            }
        }

        private static string GetPrivateKey(ECDsa ecdsa)
        {
            return Convert.ToBase64String(ecdsa.ExportPkcs8PrivateKey());
        }

        private static string GetPublicKey(ECDsa ecdsa)
        {
            return Convert.ToBase64String(ecdsa.ExportSubjectPublicKeyInfo());
        }
    }

}
