﻿using AutoFixture;
using Corely.Domain.Validators.FluentValidators;
using FluentValidation;

namespace Corely.UnitTests.Domain.Validators.FluentValidators
{
    public class FluentValidationFactoryTests
    {
        private readonly FluentValidatorFactory _factory;
        private readonly Fixture _fixture = new();

        public FluentValidationFactoryTests()
        {
            var serviceProviderMock = GetMockServiceProvider();
            _factory = new FluentValidatorFactory(serviceProviderMock);
        }

        private static IServiceProvider GetMockServiceProvider()
        {
            var validatorMock = new Mock<IValidator<string>>();
            validatorMock
                .Setup(v => v.Validate(It.IsAny<string>()))
                .Returns(new FluentValidation.Results.ValidationResult());

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(p => p.GetService(typeof(IValidator<string>)))
                .Returns(validatorMock.Object);

            return serviceProviderMock.Object;
        }

        [Fact]
        public void GetValidator_ReturnsValidator()
        {
            var validator = _factory.GetValidator<string>();
            Assert.NotNull(validator);
        }

        [Fact]
        public void GetValidator_ThrowsInvalidOperationException_WhenValidatorIsNotRegistered()
        {
            Assert.Throws<InvalidOperationException>(() => _factory.GetValidator<object>());
        }
    }
}
