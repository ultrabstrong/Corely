using AutoFixture;
using AutoMapper;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper
{
    public abstract class ProfileTestsBase<TSource, TDestination>
        : IDisposable
        where TSource : class
    {
        protected readonly IMapper mapper;
        private readonly ServiceFactory _serviceFactory = new();

        protected ProfileTestsBase()
        {
            mapper = _serviceFactory.GetRequiredService<IMapper>();
        }

        [Fact]
        public void Map_ShouldMapSourceToDestination()
        {
            var source = GetSource();
            var modifiedSource = ApplySourceModifications(source);
            mapper.Map<TDestination>(modifiedSource);
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
