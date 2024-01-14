﻿using AutoFixture;
using AutoMapper;
using Corely.Domain.Validators.FluentValidators;
using FluentValidation;
using CorelyValidationException = Corely.Domain.Exceptions.ValidationException;
using FluentValidationFailure = FluentValidation.Results.ValidationFailure;
using FluentValidationResult = FluentValidation.Results.ValidationResult;

namespace Corely.UnitTests.Domain.Validators.FluentValidators
{
    public class FluentValidationProviderTests
        : IDisposable
    {
        private const string INVALID_STRING = "invalid string";

        private readonly FluentValidationProvider _provider;
        private readonly Fixture _fixture = new();
        private readonly ServiceFactory _serviceFactory = new();

        public FluentValidationProviderTests()
        {
            var serviceProviderMock = GetMockServiceProvider();
            var fluentValidatorFactory = new FluentValidatorFactory(serviceProviderMock);

            var mapper = _serviceFactory.GetRequiredService<IMapper>();

            _provider = new FluentValidationProvider(fluentValidatorFactory, mapper);
        }

        private static IServiceProvider GetMockServiceProvider()
        {
            var validatorMock = new Mock<IValidator<string>>();

            validatorMock.Setup(v => v.Validate(It.Is<string>(s => s == INVALID_STRING)))
                .Returns(new FluentValidationResult(
                    [new FluentValidationFailure("property", "error message")]
                ));

            validatorMock
                .Setup(v => v.Validate(It.Is<string>(s => s != INVALID_STRING)))
                .Returns(new FluentValidationResult());

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(p => p.GetService(typeof(IValidator<string>)))
                .Returns(validatorMock.Object);

            return serviceProviderMock.Object;
        }

        [Fact]
        public void Validate_ReturnsValidationResult()
        {
            var toValidate = _fixture.Create<string>();
            var result = _provider.Validate(toValidate);
            Assert.NotNull(result);
        }

        [Fact]
        public void Validate_ThrowsInvalidOperationException_WhenValidatorIsNotRegistered()
        {
            var toValidate = _fixture.Create<object>();

            var ex = Record.Exception(() => _provider.Validate(toValidate));
            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Theory]
        [InlineData(INVALID_STRING)]
        [InlineData(null)]
        public void ThrowIfInvalid_ThrowsValidationException_WhenValidationFails(string? value)
        {
            var ex = Record.Exception(() => _provider.ThrowIfInvalid(value));
            Assert.NotNull(ex);
            Assert.IsType<CorelyValidationException>(ex);
        }

        public void Dispose()
        {
            _serviceFactory?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
