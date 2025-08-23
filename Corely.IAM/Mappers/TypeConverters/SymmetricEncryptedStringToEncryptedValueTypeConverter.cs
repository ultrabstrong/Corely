using Corely.IAM.Security.Factories;
using Corely.IAM.Security.Models;

namespace Corely.IAM.Mappers.TypeConverters;

internal static class SymmetricEncryptedStringToEncryptedValueTypeConverter
{
    public static ISymmetricEncryptedValue Convert(string source, ISymmetricEncryptionProviderFactory encryptionProviderFactory)
    {
        var encryptionProvider = encryptionProviderFactory.GetProviderForDecrypting(source);
        return new SymmetricEncryptedValue(encryptionProvider, source);
    }

    public static string Convert(ISymmetricEncryptedValue source)
    {
        return source?.Secret ?? string.Empty;
    }
}