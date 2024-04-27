namespace Corely.Common.Extensions
{
    public static class StringBase64EncodeExtensions
    {
        public static string Base64Encode(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(s));
        }

        public static string Base64Decode(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(s));
        }
    }
}
