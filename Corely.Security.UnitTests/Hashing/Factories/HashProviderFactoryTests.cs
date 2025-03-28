﻿using AutoFixture;
using Corely.Security.Hashing;
using Corely.Security.Hashing.Factories;
using Corely.Security.Hashing.Providers;
using Corely.TestBase.ClassData;

namespace Corely.Security.UnitTests.Hashing.Factories;

public class HashProviderFactoryTests
{
    private const string DEFAULT_PROVIDER_CODE = HashConstants.SALTED_SHA256_CODE;
    private readonly HashProviderFactory _hashProviderFactory = new(DEFAULT_PROVIDER_CODE);
    private readonly Fixture _fixture = new();

    [Fact]
    public void AddProvider_AddsProvider()
    {
        var providerCode = _fixture.Create<string>();
        var provider = new Mock<IHashProvider>().Object;

        _hashProviderFactory.AddProvider(providerCode, provider);
        var hashProvider = _hashProviderFactory.GetProvider(providerCode);

        Assert.NotNull(hashProvider);
    }

    [Fact]
    public void AddProvider_Throws_WithExistingProviderCode()
    {
        var providerCode = _fixture.Create<string>();
        var provider = new Mock<IHashProvider>().Object;

        _hashProviderFactory.AddProvider(providerCode, provider);
        var ex = Record.Exception(() => _hashProviderFactory.AddProvider(providerCode, provider));

        Assert.NotNull(ex);
        Assert.IsType<HashException>(ex);
    }

    [Theory]
    [ClassData(typeof(NullEmptyAndWhitespace))]
    [InlineData(":")]
    public void AddProvider_Throws_WithInvalidCode(string providerCode)
    {
        var provider = new Mock<IHashProvider>().Object;

        var ex = Record.Exception(() => _hashProviderFactory.AddProvider(providerCode, provider));

        Assert.NotNull(ex);
        Assert.True(ex is ArgumentNullException
            || ex is ArgumentException
            || ex is HashException);
    }

    [Fact]
    public void AddProvider_Throws_WithNullProvider()
    {
        var providerCode = _fixture.Create<string>();

        var ex = Record.Exception(() => _hashProviderFactory.AddProvider(providerCode, null));

        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void UpdateProvider_UpdatesProvider()
    {
        var providerCode = _fixture.Create<string>();
        var originalProvider = new Mock<IHashProvider>().Object;
        var updatedProvider = new Mock<IHashProvider>().Object;

        _hashProviderFactory.AddProvider(providerCode, originalProvider);
        _hashProviderFactory.UpdateProvider(providerCode, updatedProvider);
        var hashProvider = _hashProviderFactory.GetProvider(providerCode);

        Assert.Equal(updatedProvider, hashProvider);
    }

    [Fact]
    public void UpdateProvider_Throws_WithNonExistingProviderCode()
    {
        var providerCode = _fixture.Create<string>();
        var provider = new Mock<IHashProvider>().Object;

        var ex = Record.Exception(() => _hashProviderFactory.UpdateProvider(providerCode, provider));

        Assert.NotNull(ex);
        Assert.IsType<HashException>(ex);
    }

    [Theory]
    [ClassData(typeof(NullEmptyAndWhitespace))]
    [InlineData(":")]
    public void UpdateProvider_Throws_WithInvalidCode(string providerCode)
    {
        var provider = new Mock<IHashProvider>().Object;

        var ex = Record.Exception(() => _hashProviderFactory.UpdateProvider(providerCode, provider));

        Assert.NotNull(ex);
        Assert.True(ex is ArgumentNullException
            || ex is ArgumentException
            || ex is HashException);
    }

    [Fact]
    public void UpdateProvider_Throws_WithNullProvider()
    {
        var providerCode = _fixture.Create<string>();

        var ex = Record.Exception(() => _hashProviderFactory.UpdateProvider(providerCode, null));

        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void GetDefaultProvider_ReturnsDefaultProvider()
    {
        var hashProvider = _hashProviderFactory.GetDefaultProvider();
        Assert.NotNull(hashProvider);
        Assert.Equal(DEFAULT_PROVIDER_CODE, hashProvider.HashTypeCode);
    }

    [Theory, MemberData(nameof(GetProviderData))]
    public void GetProvider_ReturnProvider(string providerCode, Type expectedType)
    {
        var hashProvider = _hashProviderFactory.GetProvider(providerCode);
        Assert.NotNull(hashProvider);
        Assert.IsType(expectedType, hashProvider);
    }

    [Theory]
    [ClassData(typeof(NullEmptyAndWhitespace))]
    [InlineData("-")]
    [InlineData("--")]
    public void GetProvider_Throws_WithInvalidCode(string providerCode)
    {
        var ex = Record.Exception(() => _hashProviderFactory.GetProvider(providerCode));

        Assert.NotNull(ex);
        Assert.True(ex is ArgumentNullException
            || ex is ArgumentException
            || ex is HashException);
    }

    [Theory, MemberData(nameof(GetProviderData))]
    public void GetProviderToVerify_ReturnHashProvider(string providerCode, Type expectedType)
    {
        var fixture = new Fixture();
        var hashedValue = $"{providerCode}:{fixture.Create<string>()}";
        var hashProvider = _hashProviderFactory.GetProviderToVerify(hashedValue);
        Assert.NotNull(hashProvider);
        Assert.IsType(expectedType, hashProvider);
    }

    [Theory]
    [ClassData(typeof(NullEmptyAndWhitespace))]
    [InlineData("-")]
    [InlineData("--")]
    public void GetProviderToVerify_Throws_WithInvalidCode(string providerCode)
    {
        var ex = Record.Exception(() => _hashProviderFactory.GetProviderToVerify(providerCode));

        Assert.NotNull(ex);
        Assert.True(ex is ArgumentNullException
            || ex is ArgumentException
            || ex is HashException);
    }

    [Fact]
    public void ListProviders_ReturnsListOfProviders()
    {
        var providerCode = _fixture.Create<string>();
        var provider = new Mock<IHashProvider>().Object;

        var providers = _hashProviderFactory.ListProviders();
        _hashProviderFactory.AddProvider(providerCode, provider);
        var updatedProviders = _hashProviderFactory.ListProviders();

        Assert.NotNull(providers);
        Assert.NotEmpty(providers);
        Assert.NotNull(updatedProviders);
        Assert.NotEmpty(updatedProviders);
        Assert.True(providers.Count < updatedProviders.Count);
    }

    public static IEnumerable<object[]> GetProviderData()
    {
        yield return new object[] { HashConstants.SALTED_SHA256_CODE, typeof(Sha256SaltedHashProvider) };
        yield return new object[] { HashConstants.SALTED_SHA512_CODE, typeof(Sha512SaltedHashProvider) };
    }
}
