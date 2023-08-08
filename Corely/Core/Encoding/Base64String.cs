namespace Corely.Core.Encoding
{
    public static class Base64String
    {
        public static string Base64Encode(string s)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(s));
        }

        public static string Base64Decode(string s)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(s));
        }
    }
}
