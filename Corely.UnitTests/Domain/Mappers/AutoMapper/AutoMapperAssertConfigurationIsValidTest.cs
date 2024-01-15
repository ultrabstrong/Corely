using AutoMapper;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper
{
    public class AutoMapperAssertConfigurationIsValidTest
        : IDisposable
    {
        private readonly IMapper _mapper;
        private readonly ServiceFactory _serviceFactory = new();

        public AutoMapperAssertConfigurationIsValidTest()
        {
            _mapper = _serviceFactory.GetRequiredService<IMapper>();
        }

        [Fact]
        public void AssertConfigurationIsValid_ShouldSucceed()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        public void Dispose()
        {
            _serviceFactory?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
