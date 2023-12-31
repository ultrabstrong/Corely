using AutoFixture;
using AutoMapper;
using Corely.Domain.Mappers.AutoMapper;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper
{
    public abstract class AutoMapperTestBase<TSource, TDestination>
        where TSource : class
    {
        protected readonly IMapper _mapper;

        protected AutoMapperTestBase()
        {
            var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(AutoMapperMapProvider)));
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void MapWithoutThrowing()
        {
            var source = GetMock<TSource>();
            _mapper.Map<TDestination>(source);
        }

        protected T GetMock<T>() where T : class
        {
            try
            {
                return new Mock<T>(GetSourceParams()).Object;
            }
            catch (Exception)
            {
                return new Fixture().Create<T>();
            }
        }

        protected virtual object[] GetSourceParams() => [];
    }
}
