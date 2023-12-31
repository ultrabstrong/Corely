﻿namespace Corely.Common.Providers.Data
{
    public class RandomStringProvider : IRandomStringProvider
    {
        private Random Random { get; set; } = new Random();

        public string GetString(int length)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));

            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(
                Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)])
                .ToArray());
        }
    }
}
