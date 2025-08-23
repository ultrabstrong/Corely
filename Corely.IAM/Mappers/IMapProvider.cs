namespace Corely.IAM.Mappers;

public interface IMapProvider
{
    public TDestination? MapTo<TDestination>(object? model);
}