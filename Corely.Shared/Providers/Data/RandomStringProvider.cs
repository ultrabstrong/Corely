namespace Corely.Shared.Providers.Data
{
    public class RandomStringProvider : IRandomStringProvider
    {
        private Random Random { get; set; } = new Random();

        public string GetString(int length)
        {
            if (length < 0)
            {
                throw new ArgumentException("Length must be greater than 0.", nameof(length));
            }

            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(
                Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)])
                .ToArray());
        }
    }
}
