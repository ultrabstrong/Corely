using AutoFixture;
using AutoMapper;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper
{
    public abstract class AutoMapperTestsBase<TSource, TDestination>
        : IDisposable
        where TSource : class
    {
        protected readonly IMapper _mapper;
        private readonly ServiceFactory _serviceFactory = new();

        protected AutoMapperTestsBase()
        {
            _mapper = _serviceFactory.GetRequiredService<IMapper>();
        }

        [Fact]
        public void Map_ShouldMapSourceToDestination()
        {
            var source = GetSource();
            var modifiedSource = ApplySourceModifications(source);
            _mapper.Map<TDestination>(modifiedSource);
        }

        protected virtual TSource GetSource() => GetMock<TSource>();

        protected virtual TSource ApplySourceModifications(TSource source) => source;

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

        public void Dispose()
        {
            _serviceFactory?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
