using AutoMapper;
using Corely.Domain.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper
{
    public class AutoMapperPrebuiltConfigValidationTest
    {
        private readonly IMapper _mapper;

        public AutoMapperPrebuiltConfigValidationTest()
        {
            var services = new ServiceCollection();

            services.AddAutoMapper(typeof(IMapProvider).Assembly);

            _mapper = services.BuildServiceProvider()
                .GetRequiredService<IMapper>();
        }

        [Fact]
        public void AssertConfigurationIsValid_ShouldSucceed()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
