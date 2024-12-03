using Corely.Security.Hashing.Providers;

namespace Corely.Security.Hashing.Models;

public class HashedValue : IHashedValue
{
    private readonly object _lock = new();
    private readonly IHashProvider _hashProvider;

    public HashedValue(IHashProvider hashProvider)
    {
        ArgumentNullException.ThrowIfNull(hashProvider, nameof(hashProvider));
        _hashProvider = hashProvider;
    }

    public string Hash
    {
        get => _hash;
        init => _hash = value;
    }
    private string _hash = string.Empty;

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
