namespace Corely.IAM.Mappers
{
    internal interface IMapProvider
    {
        public TDestination MapTo<TDestination>(object model);
    }
}
