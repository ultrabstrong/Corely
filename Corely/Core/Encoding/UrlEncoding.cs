namespace Corely.Core.Encoding
{
    public static class UrlEncoding
    {
        public static string UrlEncode(this string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source cannot be null");
            }
            return Uri.EscapeDataString(source);
        }

        public static string UrlDecode(this string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source cannot be null");
            }
            source = source.Replace("+", "%20");
            return Uri.UnescapeDataString(source);
        }

    }
}
