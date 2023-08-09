namespace Corely.Shared.Core.Encoding
{
    public static class UrlEncoding
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
}
