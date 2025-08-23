using Corely.IAM.Groups.Entities;
using Corely.IAM.Groups.Models;
using Corely.IAM.Mappers;
using Corely.IAM.Users.Models;

namespace Corely.IAM.UnitTests.Mappers;

public class CustomMapProviderTests
{
    [Fact]
    public void MapTo_ReturnsNull_WithNullArg()
    {
        var mapper = new ServiceFactory().GetRequiredService<IMapProvider>();
        var response = mapper.MapTo<User>(null!);
        Assert.Null(response);
    }

    [Fact]
    public void MapTo_MapsGroupToEntity_Successfully()
    {
        var mapper = new ServiceFactory().GetRequiredService<IMapProvider>();
        var group = new Group { Id = 1, Name = "Test Group", Description = "Test Description" };
        
        var entity = mapper.MapTo<GroupEntity>(group);
        
        Assert.NotNull(entity);
        Assert.Equal(group.Id, entity.Id);
        Assert.Equal(group.Name, entity.Name);
        Assert.Equal(group.Description, entity.Description);
    }

    [Fact]
    public void MapTo_MapsEntityToGroup_Successfully()
    {
        var mapper = new ServiceFactory().GetRequiredService<IMapProvider>();
        var entity = new GroupEntity { Id = 1, Name = "Test Group", Description = "Test Description" };
        
        var group = mapper.MapTo<Group>(entity);
        
        Assert.NotNull(group);
        Assert.Equal(entity.Id, group.Id);
        Assert.Equal(entity.Name, group.Name);
        Assert.Equal(entity.Description, group.Description);
    }

    [Fact]
    public void MapTo_ThrowsException_ForUnmappedType()
    {
        var mapper = new ServiceFactory().GetRequiredService<IMapProvider>();
        var source = new { Id = 1, Name = "Test" };
        
        var ex = Assert.Throws<InvalidOperationException>(() => mapper.MapTo<Group>(source));
        Assert.Contains("No mapping found", ex.Message);
    }
}