using Corely.IAM.Mappers;
using Corely.IAM.Security.Entities;
using Corely.IAM.Security.Models;

namespace Corely.IAM.UnitTests.Security.Mappers;

public class AsymmetricKeyMapperTests
{
    private readonly ServiceFactory _serviceFactory = new();

    [Fact]
    public void ToEntity_MapsSourceToDestination()
    {
        var mapProvider = _serviceFactory.GetRequiredService<IMapProvider>();
        var source = new AsymmetricKey
        {
            Id = 1,
            PublicKey = "public-key",
            PrivateKey = "private-key",
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(30)
        };

        var result = mapProvider.MapTo<AsymmetricKeyEntity>(source);

        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.PublicKey, result.PublicKey);
        Assert.NotEmpty(result.EncryptedPrivateKey);
        Assert.Equal(source.CreatedAt, result.CreatedAt);
        Assert.Equal(source.ExpiresAt, result.ExpiresAt);
    }

    [Fact]
    public void ToModel_MapsSourceToDestination()
    {
        var mapProvider = _serviceFactory.GetRequiredService<IMapProvider>();
        var source = new AsymmetricKeyEntity
        {
            Id = 1,
            PublicKey = "public-key",
            EncryptedPrivateKey = "encrypted-private-key",
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(30)
        };

        var result = mapProvider.MapTo<AsymmetricKey>(source);

        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.PublicKey, result.PublicKey);
        Assert.NotEmpty(result.PrivateKey);
        Assert.Equal(source.CreatedAt, result.CreatedAt);
        Assert.Equal(source.ExpiresAt, result.ExpiresAt);
    }

    [Fact]
    public void MapTo_ReturnsNull_WhenSourceIsNull()
    {
        var mapProvider = _serviceFactory.GetRequiredService<IMapProvider>();
        var result = mapProvider.MapTo<AsymmetricKeyEntity>(null);
        Assert.Null(result);
    }
}