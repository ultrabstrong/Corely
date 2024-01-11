using Corely.Domain.Mappers;
using Corely.Domain.Services;
using Corely.Domain.Services.Users;
using Corely.Domain.Validators;
using Corely.UnitTests.Collections;
using Corely.UnitTests.Fixtures;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.Domain.Services
{
    [Collection(CollectionNames.ServiceFactory)]
    public class ServiceBaseTests
    {
        private class MockServiceBase : ServiceBase
        {
            public MockServiceBase(
                IMapProvider mapProvider,
                IValidationProvider validationProvider,
                ILogger logger)
                : base(mapProvider, validationProvider, logger)
            {
            }
        }

        private readonly ServiceFactory _serviceFactory;
        private readonly MockServiceBase _mockServiceBase;

        public ServiceBaseTests(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
            _mockServiceBase = new MockServiceBase(
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.CreateLogger<UserService>());
        }

        [Fact]
        public void MapToValid_ShouldThrowIfSourceIsNull()
        {
            var ex = Record.Exception(() => _mockServiceBase.MapToValid<object>(null!));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }
    }
}
