using System.Diagnostics.CodeAnalysis;

namespace Corely.Shared.Providers.Security.Encryption
{
    public static class EncryptionProviderConstants
    {
        [StringSyntax(StringSyntaxAttribute.Regex)]
        public const string ENCRYPTION_TYPE_CODE_REGEX = @"^\d{2}";

        public const string Aes = "00";
    }
}
