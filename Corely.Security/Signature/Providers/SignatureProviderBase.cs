using Corely.Security.KeyStore;

namespace Corely.Security.Signature.Providers
{
    public abstract class SignatureProviderBase : ISignatureProvider
    {
        public abstract string SignatureTypeCode { get; }

        public SignatureProviderBase()
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

        public string Sign(string data, IAsymmetricEncryptionKeyStoreProvider keyStoreProvider)
        {
            ArgumentNullException.ThrowIfNull(data, nameof(data));
            var (_, privateKey) = keyStoreProvider.GetCurrentKeys();
            return SignInternal(data, privateKey);
        }

        public bool Verify(string data, string signature, IAsymmetricEncryptionKeyStoreProvider keyStoreProvider)
        {
            ArgumentNullException.ThrowIfNull(data, nameof(data));
            ArgumentNullException.ThrowIfNull(signature, nameof(signature));
            var (publicKey, _) = keyStoreProvider.GetCurrentKeys();
            return VerifyInternal(data, signature, publicKey);
        }

        protected abstract string SignInternal(string value, string privateKey);

        protected abstract bool VerifyInternal(string value, string signature, string publicKey);
    }
}
