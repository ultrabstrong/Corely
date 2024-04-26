namespace Corely.UnitTests.IAM.Mappers.AutoMapper.Profiles
{
    public abstract class BidirectionalProfileTestsBase<TSource, TDestination>
        : ProfileTestsBase<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        [Fact]
        public void ReverseMap_ShouldMapDestinationToSource()
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
