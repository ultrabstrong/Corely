using AutoFixture;
using Corely.IAM.Mappers;
using Moq;

namespace Corely.IAM.UnitTests.Mappers;

public abstract class MapperTestsBase
{
    [Fact]
    public abstract void Map_MapsSourceToDestination();

    protected static T GetMock<T>(object[] args)
        where T : class
    {
        try
        {
            return new Mock<T>(args).Object;
        }
        catch (Exception)
        {
            return new Fixture().Create<T>();
        }
    }
}

public abstract class MapperDelegateTestsBase
    : MapperTestsBase
{
    protected abstract MapperTestsBase GetDelegate();

    [Fact]
    public override void Map_MapsSourceToDestination()
        => GetDelegate().Map_MapsSourceToDestination();
}

public abstract class MapperTestsBase<TSource, TDestination>
    : MapperTestsBase
    where TSource : class
    where TDestination : class
{
    private readonly ServiceFactory _serviceFactory = new();

    protected MapperTestsBase()
    {
    }

    [Fact]
    public override void Map_MapsSourceToDestination()
    {
        var source = GetSource();
        var modifiedSource = ApplySourceModifications(source);
        
        // This will test the actual mapping through our custom provider
        var mapProvider = _serviceFactory.GetRequiredService<IMapProvider>();
        var result = mapProvider.MapTo<TDestination>(modifiedSource);
        
        Assert.NotNull(result);
    }

    protected virtual TSource GetSource() => GetMock<TSource>(GetSourceParams());

    protected virtual TSource ApplySourceModifications(TSource source) => source;

    protected virtual object[] GetSourceParams() => [];
}