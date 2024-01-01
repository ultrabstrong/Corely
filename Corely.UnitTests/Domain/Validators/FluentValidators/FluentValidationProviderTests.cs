using AutoFixture;
using AutoMapper;
using Corely.Domain.Validators;
using Corely.Domain.Validators.FluentValidators;
using FluentValidation;

namespace Corely.UnitTests.Domain.Validators.FluentValidators
{
    public class FluentValidationProviderTests
    {
        private readonly FluentValidationProvider _provider;
        private readonly Fixture _fixture = new();

        public FluentValidationProviderTests()
        {
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<ValidationResult>(
                    It.IsAny<FluentValidation.Results.ValidationResult>()))
                .Returns(new ValidationResult());

            var serviceProviderMock = GetMockServiceProvider();
            var fluentValidatorFactory = new FluentValidatorFactory(serviceProviderMock);

            _provider = new FluentValidationProvider(fluentValidatorFactory, mapperMock.Object);
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
            Assert.Throws<InvalidOperationException>(() => _provider.Validate(toValidate));
        }
    }
}
