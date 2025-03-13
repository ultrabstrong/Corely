using AutoFixture;
using Corely.Security.Hashing.Providers;
using Corely.TestBase.ClassData;

namespace Corely.Security.UnitTests.Hashing.Providers;

public abstract class SaltedHashProviderGenericTests
{
    protected abstract IHashProvider HashProvider { get; }

    private readonly Fixture _fixture = new();

    [Fact]
    public void Hash_ReturnsHashedValue()
    {
        var value = _fixture.Create<string>();
        var hashed = HashProvider.Hash(value);
        Assert.NotEqual(value, hashed);
    }

    [Fact]
    public void Hash_ProducesDifferentValues_WithSameInput()
    {
        var value = _fixture.Create<string>();
        var hash1 = HashProvider.Hash(value);
        var hash2 = HashProvider.Hash(value);
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Hash_ReturnsCorrectlyFormattedValue()
    {
        var value = _fixture.Create<string>();
        var hashed = HashProvider.Hash(value);
        Assert.StartsWith(HashProvider.HashTypeCode, hashed);
        Assert.NotEqual(value, hashed[hashed.IndexOf(':')..]);
    }

    [Theory, ClassData(typeof(EmptyAndWhitespace))]
    public void Hash_ReturnsCorrectlyFormattedValue_WithEmptyAndWhitespace(string value)
    {
        var hashed = HashProvider.Hash(value);
        Assert.StartsWith(HashProvider.HashTypeCode, hashed);
        Assert.NotEqual(value, hashed[hashed.IndexOf(':')..]);
    }

    [Fact]
    public void Hash_Throws_WithNullInput()
    {
        var ex = Record.Exception(() => HashProvider.Hash(null));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void Verify_ReturnsTrue_WithMatchingValueHash()
    {
        var value = _fixture.Create<string>();
        var hash = HashProvider.Hash(value);
        var isVerified = HashProvider.Verify(value, hash);
        Assert.True(isVerified);
    }

    [Fact]
    public void Verify_ReturnsFalse_WithNonMatchingHash()
    {
        var value = _fixture.Create<string>();
        var hash = HashProvider.Hash(value);
        var isVerified = HashProvider.Verify(value + "1", hash);
        Assert.False(isVerified);
    }

    [Fact]
    public void Verify_ReturnsFalse_WithNonMatchingHashType()
    {
        var value = _fixture.Create<string>();
        var hash = HashProvider.Hash(value);
        var isVerified = HashProvider.Verify(value, hash.Replace(HashProvider.HashTypeCode, "--"));
        Assert.False(isVerified);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(null, "")]
    [InlineData("", null)]
    public void Verify_Throws_WithNull(string? value, string? hash)
    {
        var ex = Record.Exception(() => HashProvider.Verify(value!, hash!));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public abstract void HashTypeCode_ReturnsCorrectCode_ForImplementation();
}
