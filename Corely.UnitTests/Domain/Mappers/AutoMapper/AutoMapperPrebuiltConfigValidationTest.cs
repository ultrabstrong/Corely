using AutoMapper;
using Corely.UnitTests.Fixtures;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper
{
    public class AutoMapperPrebuiltConfigValidationTest
        : IDisposable
    {
        private readonly IMapper _mapper;
        private readonly ServiceFactory _serviceFactory = new();

        public AutoMapperPrebuiltConfigValidationTest()
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
