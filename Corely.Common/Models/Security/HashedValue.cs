using Corely.Common.Extensions;
using Corely.Common.Providers.Security.Hashing;

namespace Corely.Common.Models.Security
{
    public class HashedValue(
        IHashProvider hashProvider,
        string hash)
        : IHashedValue
    {
        private readonly object _lock = new();
        private readonly IHashProvider _hashProvider = hashProvider.ThrowIfNull(nameof(hashProvider));
        public string Hash { get; private set; } = hash;

        public HashedValue(IHashProvider hashProvider)
            : this(hashProvider, "") { }

        public void Set(string value)
        {
            var hashedValue = _hashProvider.Hash(value);
            lock (_lock)
            {
                Hash = hashedValue;
            }
        }

        public bool Verify(string value)
        {
            return _hashProvider.Verify(value, Hash);
        }
    }
}
