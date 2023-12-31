namespace Corely.UnitTests.Domain.Mappers.AutoMapper
{
    public abstract class BidirectionalAutoMapperTestsBase<TSource, TDestination>
        : AutoMapperTestBase<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        [Fact]
        public void MapOtherDirectionWithoutThrowing()
        {
            var destination = GetMock<TDestination>();
            _mapper.Map<TSource>(destination);
        }

        protected virtual object[] GetDestinationParams() => [];
    }
}
