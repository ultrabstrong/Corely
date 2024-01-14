﻿using AutoFixture;
using AutoMapper;
using Corely.Domain.Exceptions;
using Corely.Domain.Mappers;
using Corely.Domain.Models.Users;
using Corely.Domain.Services;
using Corely.Domain.Validators;
using Corely.UnitTests.Collections;
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

        private const string VALID_USERNAME = "username";
        private const string VALID_EMAIL = "email@x.y";

        private readonly Fixture _fixutre = new();
        private readonly ServiceFactory _serviceFactory;
        private readonly MockServiceBase _mockServiceBase;

        public ServiceBaseTests(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
            _mockServiceBase = new MockServiceBase(
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<ILogger<ServiceBaseTests>>());
        }

        [Fact]
        public void MapToValid_ShouldThrowIfSourceIsNull()
        {
            var ex = Record.Exception(() => _mockServiceBase.MapToValid<object>(null!));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void MapToValid_ShouldThrowAutoMapperMappingException_IfDestinationIsInvalid()
        {
            var createUserRequest = _fixutre.Create<CreateUserRequest>();
            var ex = Record.Exception(() => _mockServiceBase.MapToValid<CreateUserResult>(createUserRequest));

            Assert.NotNull(ex);
            Assert.IsType<AutoMapperMappingException>(ex);
        }

        [Fact]
        public void MapToValid_ShouldThrowValidationException_IfDestinationIsInvalid()
        {
            var createUserRequest = _fixutre.Create<CreateUserRequest>();
            var ex = Record.Exception(() => _mockServiceBase.MapToValid<User>(createUserRequest));

            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
        }

        [Fact]
        public void MapToValid_ShouldReturnValidDestination_IfSourceIsValid()
        {
            var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
            var user = _mockServiceBase.MapToValid<User>(createUserRequest);

            Assert.NotNull(user);
            Assert.Equal(createUserRequest.Username, user.Username);
            Assert.Equal(createUserRequest.Email, user.Email);
        }
    }
}
