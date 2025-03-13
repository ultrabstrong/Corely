using AutoMapper;
using Corely.IAM.Mappers;
using Corely.IAM.Mappers.AutoMapper;
using Corely.IAM.Users.Models;

namespace Corely.IAM.UnitTests.Mappers.AutoMapper;

public class AutoMapperMapProviderTests
{
    [Fact]
    public void Map_CallsWrappedMapper()
    {
        var autoMapperMock = new Mock<IMapper>();
        var provider = new AutoMapperMapProvider(autoMapperMock.Object);

        var response = provider.MapTo<object>(new object());

        autoMapperMock.Verify(m => m.Map<object>(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public void MapTo_ReturnsNull_WithNullArg()
    {
        var mapper = new ServiceFactory().GetRequiredService<IMapProvider>();
        var response = mapper.MapTo<User>(null!);
        Assert.Null(response);
    }
}
