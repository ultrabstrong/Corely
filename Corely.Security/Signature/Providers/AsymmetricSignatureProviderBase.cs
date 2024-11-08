using Corely.Security.Keys;
using Corely.Security.KeyStore;
using Microsoft.IdentityModel.Tokens;

namespace Corely.Security.Signature.Providers
{
    public abstract class AsymmetricSignatureProviderBase : IAsymmetricSignatureProvider
    {
        public abstract string SignatureTypeCode { get; }

        public AsymmetricSignatureProviderBase()
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

        public string Sign(string data, IAsymmetricKeyStoreProvider keyStoreProvider)
        {
            ArgumentNullException.ThrowIfNull(data, nameof(data));
            var (_, privateKey) = keyStoreProvider.GetCurrentKeys();
            return SignInternal(data, privateKey);
        }

        public bool Verify(string data, string signature, IAsymmetricKeyStoreProvider keyStoreProvider)
        {
            ArgumentNullException.ThrowIfNull(data, nameof(data));
            ArgumentNullException.ThrowIfNull(signature, nameof(signature));
            var (publicKey, _) = keyStoreProvider.GetCurrentKeys();
            return VerifyInternal(data, signature, publicKey);
        }

        public abstract IAsymmetricKeyProvider GetAsymmetricKeyProvider();

        public abstract SigningCredentials GetSigningCredentials(string key, bool isKeyPrivate);

        protected abstract string SignInternal(string value, string privateKey);

        protected abstract bool VerifyInternal(string value, string signature, string publicKey);
    }
}
