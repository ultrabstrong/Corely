using AutoFixture;
using AutoMapper;
using Corely.IAM.Mappers;
using Corely.IAM.Models;
using Corely.IAM.Processors;
using Corely.IAM.Users.Models;
using Corely.IAM.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.Processors
{
    public class ProcessorBaseTests
    {
        private class MockProcessorBase : ProcessorBase
        {
            public MockProcessorBase(
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
        private readonly MockProcessorBase _mockProcessorBase;

        public ProcessorBaseTests()
        {
            _mockProcessorBase = new MockProcessorBase(
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<ILogger<ProcessorBaseTests>>());
        }

        [Fact]
        public void MapThenValidateTo_ReturnsValidDestination_IfSourceIsValid()
        {
            var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
            var user = _mockProcessorBase.MapThenValidateTo<User>(createUserRequest);

            Assert.NotNull(user);
            Assert.Equal(createUserRequest.Username, user.Username);
            Assert.Equal(createUserRequest.Email, user.Email);
        }

        [Fact]
        public void MapThenValidateTo_Throws_WhenSourceIsNull()
        {
            var ex = Record.Exception(() => _mockProcessorBase.MapThenValidateTo<object>(null!));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void MapThenValidateTo_Throws_IfDestinationIsUnmapped()
        {
            var createUserRequest = _fixture.Create<CreateUserRequest>();

            var ex = Record.Exception(() => _mockProcessorBase.MapThenValidateTo<CreateResult>(createUserRequest));

            Assert.NotNull(ex);
            Assert.IsType<AutoMapperMappingException>(ex);
        }

        [Fact]
        public void MapThenValidateTo_Throws_IfDestinationIsInvalid()
        {
            var createUserRequest = _fixture.Create<CreateUserRequest>();

            var ex = Record.Exception(() => _mockProcessorBase.MapThenValidateTo<User>(createUserRequest));

            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
        }

        [Fact]
        public void MapTo_ReturnsValidDestination_IfSourceIsValid()
        {
            var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
            var user = _mockProcessorBase.MapTo<User>(createUserRequest);

            Assert.NotNull(user);
            Assert.Equal(createUserRequest.Username, user.Username);
            Assert.Equal(createUserRequest.Email, user.Email);
        }

        [Fact]
        public void MapTo_Throws_WhenSourceIsNull()
        {
            var ex = Record.Exception(() => _mockProcessorBase.MapTo<object>(null!));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void MapTo_Throws_IfDestinationIsInvalid()
        {
            var createUserRequest = _fixture.Create<CreateUserRequest>();

            var ex = Record.Exception(() => _mockProcessorBase.MapTo<CreateResult>(createUserRequest));

            Assert.NotNull(ex);
            Assert.IsType<AutoMapperMappingException>(ex);
        }

        [Fact]
        public void Validate_DoesNotThrowException_IfModelIsValid()
        {
            var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
            var user = _mockProcessorBase.MapTo<User>(createUserRequest);

            var ex = Record.Exception(() => _mockProcessorBase.Validate(user));

            Assert.Null(ex);
        }

        [Fact]
        public void Validate_Throws_WhenModelIsNull()
        {
            var ex = Record.Exception(() => _mockProcessorBase.Validate<object>(null!));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }
    }
}
