namespace Corely.Shared.Text
{
    public static class RandomStringGenerator
    {
        private static Random Random { get; set; } = new Random();

        public static string GetString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}
