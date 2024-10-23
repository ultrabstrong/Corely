using Corely.Security.Keys;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace Corely.Security.Signature.Providers
{
    public sealed class HmacSha256SignatureProvider : SymmetricSignatureProviderBase
    {
        public override string SignatureTypeCode => SymmetricSignatureConstants.HMAC_SHA256_CODE;

        private readonly RandomKeyProvider _randomKeyProvider = new();

        protected override string SignInternal(string value, string key)
        {
            var keyBytes = Convert.FromBase64String(key);
            var dataToSign = Encoding.UTF8.GetBytes(value);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                var signedBytes = hmac.ComputeHash(dataToSign);
                return Convert.ToBase64String(signedBytes);
            }
        }

        protected override bool VerifyInternal(string value, string signature, string key)
        {
            var keyBytes = Convert.FromBase64String(key);
            var dataToVerify = Encoding.UTF8.GetBytes(value);
            var signatureBytes = Convert.FromBase64String(signature);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                var computedSignatureBytes = hmac.ComputeHash(dataToVerify);
                return signatureBytes.SequenceEqual(computedSignatureBytes);
            }
        }

        public override SigningCredentials GetSigningCredentials(string key)
        {
            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(key));
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        public override ISymmetricKeyProvider GetSymmetricKeyProvider() => _randomKeyProvider;
    }
}
