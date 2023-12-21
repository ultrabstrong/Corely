namespace Corely.Domain.Mappers
{
    public interface IMapProvider
    {
        public TDestination Map<TDestination>(object model);
    }
}
