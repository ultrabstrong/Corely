using Corely.Common.Extensions;
using Corely.Common.Providers.Security.Hashing;

namespace Corely.Common.Models.Security
{
    public class HashedValue(IHashProvider hashProvider) : IHashedValue
    {
        private readonly object _lock = new();
        private readonly IHashProvider _hashProvider = hashProvider.ThrowIfNull(nameof(hashProvider));
        public string Hash
        {
            get => _hash;
            init => _hash = value;
        }
        private string _hash = "";

        public IHashedValue Set(string value)
        {
            var hashedValue = _hashProvider.Hash(value);
            lock (_lock)
            {
                _hash = hashedValue;
            }
            return this;
        }

        public bool Verify(string value)
        {
            return _hashProvider.Verify(value, Hash);
        }
    }
}
