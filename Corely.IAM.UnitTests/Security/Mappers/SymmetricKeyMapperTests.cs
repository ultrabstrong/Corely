using Corely.IAM.Mappers;
using Corely.IAM.Security.Entities;
using Corely.IAM.Security.Models;

namespace Corely.IAM.UnitTests.Security.Mappers;

public class SymmetricKeyMapperTests
{
    private readonly ServiceFactory _serviceFactory = new();

    [Fact]
    public void ToEntity_MapsSourceToDestination()
    {
        var mapProvider = _serviceFactory.GetRequiredService<IMapProvider>();
        var source = new SymmetricKey
        {
            Id = 1,
            Key = "test-key",
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(30)
        };

        var result = mapProvider.MapTo<SymmetricKeyEntity>(source);

        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.NotEmpty(result.EncryptedKey);
        Assert.Equal(source.CreatedAt, result.CreatedAt);
        Assert.Equal(source.ExpiresAt, result.ExpiresAt);
    }

    [Fact]
    public void ToModel_MapsSourceToDestination()
    {
        var mapProvider = _serviceFactory.GetRequiredService<IMapProvider>();
        var source = new SymmetricKeyEntity
        {
            Id = 1,
            EncryptedKey = "encrypted-key",
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(30)
        };

        var result = mapProvider.MapTo<SymmetricKey>(source);

        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.NotEmpty(result.Key);
        Assert.Equal(source.CreatedAt, result.CreatedAt);
        Assert.Equal(source.ExpiresAt, result.ExpiresAt);
    }

    [Fact]
    public void MapTo_ReturnsNull_WhenSourceIsNull()
    {
        var mapProvider = _serviceFactory.GetRequiredService<IMapProvider>();
        var result = mapProvider.MapTo<SymmetricKeyEntity>(null);
        Assert.Null(result);
    }
}