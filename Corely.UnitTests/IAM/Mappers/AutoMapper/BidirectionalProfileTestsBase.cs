namespace Corely.UnitTests.IAM.Mappers.AutoMapper
{
    public abstract class BidirectionalProfileTestsBase<TSource, TDestination>
        : ProfileTestsBase<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        [Fact]
        public void ReverseMap_MapsDestinationToSource()
        {
            var destination = GetDestination();
            var modifiedDestination = ApplyDestinatonModifications(destination);
            mapper.Map<TSource>(modifiedDestination);
        }

        protected virtual TDestination GetDestination() => GetMock<TDestination>();

        protected virtual TDestination ApplyDestinatonModifications(TDestination destination) => destination;

        protected virtual object[] GetDestinationParams() => [];
    }
}
