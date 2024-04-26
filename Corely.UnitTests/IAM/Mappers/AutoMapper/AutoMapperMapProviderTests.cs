using AutoMapper;
using Corely.IAM.Mappers.AutoMapper;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper
{
    public class AutoMapperMapProviderTests
    {
        [Fact]
        public void Map_ShouldCallWrappedMapper()
        {
            var autoMapperMock = new Mock<IMapper>();
            var provider = new AutoMapperMapProvider(autoMapperMock.Object);

            var response = provider.Map<object>(new object());

            autoMapperMock.Verify(m => m.Map<object>(It.IsAny<object>()), Times.Once);
        }
    }
}
