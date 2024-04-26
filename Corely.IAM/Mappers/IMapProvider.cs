namespace Corely.IAM.Mappers
{
    public interface IMapProvider
    {
        public TDestination Map<TDestination>(object model);
    }
}
