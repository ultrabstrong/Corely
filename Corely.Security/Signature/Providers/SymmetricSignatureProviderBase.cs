using Corely.Security.Keys;
using Corely.Security.KeyStore;
using Microsoft.IdentityModel.Tokens;

namespace Corely.Security.Signature.Providers
{
    public abstract class SymmetricSignatureProviderBase : ISymmetricSignatureProvider
    {
        public abstract string SignatureTypeCode { get; }

        public SymmetricSignatureProviderBase()
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(SignatureTypeCode, nameof(SignatureTypeCode));

            if (SignatureTypeCode.Contains(':'))
            {
                throw new SignatureException($"Signature type code cannot contain ':'")
                {
                    Reason = SignatureException.ErrorReason.InvalidTypeCode
                };
            }
        }

        public string Sign(string data, ISymmetricKeyStoreProvider keyStoreProvider)
        {
            ArgumentNullException.ThrowIfNull(data, nameof(data));
            var key = keyStoreProvider.GetCurrentKey();
            return SignInternal(data, key);
        }

        public bool Verify(string data, string signature, ISymmetricKeyStoreProvider keyStoreProvider)
        {
            ArgumentNullException.ThrowIfNull(data, nameof(data));
            ArgumentNullException.ThrowIfNull(signature, nameof(signature));
            var key = keyStoreProvider.GetCurrentKey();
            return VerifyInternal(data, signature, key);

        }

        public abstract ISymmetricKeyProvider GetSymmetricKeyProvider();

        public abstract SigningCredentials GetSigningCredentials(string key);

        protected abstract string SignInternal(string value, string key);

        protected abstract bool VerifyInternal(string value, string signature, string key);
    }
}
