using AutoMapper;

namespace Corely.UnitTests.Mappers.AutoMapper;

public class AutoMapperAssertConfigurationIsValidTest
{
    private readonly IMapper _mapper;
    private readonly ServiceFactory _serviceFactory = new();

    public AutoMapperAssertConfigurationIsValidTest()
    {
        _mapper = _serviceFactory.GetRequiredService<IMapper>();
    }

    [Fact]
    public void AssertConfigurationIsValid_Succeeds()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
