namespace Corely.Common.Extensions;

public static class StringUrlEncodeExtensions
{
    public static string UrlEncode(this string source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return Uri.EscapeDataString(source);
    }

    public static string UrlDecode(this string source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return Uri.UnescapeDataString(source);
    }

}
