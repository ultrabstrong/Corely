using AutoFixture;
using AutoMapper;
using Corely.IAM.Mappers;
using Corely.IAM.Models;
using Corely.IAM.Services;
using Corely.IAM.Users.Models;
using Corely.IAM.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.Services
{
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

        private const string VALID_USERNAME = "username";
        private const string VALID_EMAIL = "email@x.y";

        protected readonly ServiceFactory _serviceFactory = new();

        private readonly Fixture _fixture = new();
        private readonly MockServiceBase _mockServiceBase;

        public ServiceBaseTests()
        {
            _mockServiceBase = new MockServiceBase(
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<ILogger<ServiceBaseTests>>());
        }

        [Fact]
        public void MapAndValidate_ShouldThrowIfSourceIsNull()
        {
            var ex = Record.Exception(() => _mockServiceBase.MapAndValidate<object>(null!));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void MapAndValidate_ShouldThrowAutoMapperMappingException_IfDestinationIsInvalid()
        {
            var createUserRequest = _fixture.Create<CreateUserRequest>();

            var ex = Record.Exception(() => _mockServiceBase.MapAndValidate<CreateResult>(createUserRequest));

            Assert.NotNull(ex);
            Assert.IsType<AutoMapperMappingException>(ex);
        }

        [Fact]
        public void MapAndValidate_ShouldThrowValidationException_IfDestinationIsInvalid()
        {
            var createUserRequest = _fixture.Create<CreateUserRequest>();

            var ex = Record.Exception(() => _mockServiceBase.MapAndValidate<User>(createUserRequest));

            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
        }

        [Fact]
        public void MapAndValidate_ShouldReturnValidDestination_IfSourceIsValid()
        {
            var createUserRequest = new CreateUserRequest(_fixture.Create<int>(), VALID_USERNAME, VALID_EMAIL);
            var user = _mockServiceBase.MapAndValidate<User>(createUserRequest);

            Assert.NotNull(user);
            Assert.Equal(createUserRequest.Username, user.Username);
            Assert.Equal(createUserRequest.Email, user.Email);
        }

        [Fact]
        public void Map_ShouldReturnValidDestination_IfSourceIsValid()
        {
            var createUserRequest = new CreateUserRequest(_fixture.Create<int>(), VALID_USERNAME, VALID_EMAIL);
            var user = _mockServiceBase.Map<User>(createUserRequest);

            Assert.NotNull(user);
            Assert.Equal(createUserRequest.Username, user.Username);
            Assert.Equal(createUserRequest.Email, user.Email);
        }

        [Fact]
        public void MapOrNull_ShouldReturnNull_IfSourceIsNull()
        {
            var user = _mockServiceBase.MapOrNull<User>(null);

            Assert.Null(user);
        }

        [Fact]
        public void MapOrNull_ShouldReturnValidDestination_IfSourceIsValid()
        {
            var createUserRequest = new CreateUserRequest(_fixture.Create<int>(), VALID_USERNAME, VALID_EMAIL);
            var user = _mockServiceBase.MapOrNull<User>(createUserRequest);

            Assert.NotNull(user);
            Assert.Equal(createUserRequest.Username, user.Username);
            Assert.Equal(createUserRequest.Email, user.Email);
        }
    }
}
