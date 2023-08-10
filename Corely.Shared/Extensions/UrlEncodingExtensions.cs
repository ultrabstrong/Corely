namespace Corely.Shared.Extensions
{
    public static class UrlEncodingExtensions
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
