using Corely.Security.Keys;
using System.Security.Cryptography;
using System.Text;

namespace Corely.Security.Signature.Providers
{
    public sealed class ECDsaSignatureProvider : AsymmetricSignatureProviderBase
    {
        public override string SignatureTypeCode => AsymmetricSignatureConstants.ECDSA_SHA256_CODE;

        private readonly EcdsaKeyProvider _ecdsaKeyProvider = new();
        private readonly HashAlgorithmName _hashAlgorithm;

        public ECDsaSignatureProvider(HashAlgorithmName hashAlgorithm)
        {
            _hashAlgorithm = hashAlgorithm;
        }

        protected override string SignInternal(string value, string privateKey)
        {
            var privateKeyBytes = Convert.FromBase64String(privateKey);
            var dataToSign = Encoding.UTF8.GetBytes(value);

            using (var ecdsa = ECDsa.Create())
            {
                ecdsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
                var signedBytes = ecdsa.SignData(dataToSign, _hashAlgorithm);
                return Convert.ToBase64String(signedBytes);
            }
        }

        protected override bool VerifyInternal(string value, string signature, string publicKey)
        {
            var publicKeyBytes = Convert.FromBase64String(publicKey);
            var dataToVerify = Encoding.UTF8.GetBytes(value);
            var signatureBytes = Convert.FromBase64String(signature);

            using (var ecdsa = ECDsa.Create())
            {
                ecdsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
                return ecdsa.VerifyData(dataToVerify, signatureBytes, _hashAlgorithm);
            }
        }

        public override IAsymmetricKeyProvider GetAsymmetricKeyProvider() => _ecdsaKeyProvider;
    }
}
