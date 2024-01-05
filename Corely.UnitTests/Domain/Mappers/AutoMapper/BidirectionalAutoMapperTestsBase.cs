namespace Corely.UnitTests.Domain.Mappers.AutoMapper
{
    public abstract class BidirectionalAutoMapperTestsBase<TSource, TDestination>
        : AutoMapperTestsBase<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        [Fact]
        public void ReverseMap_ShouldMapDestinationToSource()
        {
            var destination = GetDestination();
            var modifiedDestination = ApplyDestinatonModifications(destination);
            _mapper.Map<TSource>(modifiedDestination);
        }

        protected virtual TDestination GetDestination() => GetMock<TDestination>();

        protected virtual TDestination ApplyDestinatonModifications(TDestination destination) => destination;

        protected virtual object[] GetDestinationParams() => [];
    }
}
