using Corely.DataAccess.Interfaces.Repos;
using Corely.DataAccess.Interfaces.UnitOfWork;
using Corely.IAM;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Accounts.Services;
using Corely.IAM.Auth.Services;
using Corely.IAM.Mappers;
using Corely.IAM.Security.Services;
using Corely.IAM.Services;
using Corely.IAM.Users.Services;
using Corely.IAM.Validators;
using Corely.Security.Encryption.Factories;
using Corely.Security.Hashing.Factories;
using Corely.Security.PasswordValidation.Providers;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM
{
    public abstract class ServiceFactoryGenericTests
    {
        protected abstract ServiceFactoryBase ServiceFactory { get; }

        [Theory, MemberData(nameof(GetRequiredServiceData))]
        public void ServiceFactoryBase_ProvidesService(Type serviceType)
        {
            var service = GetRequiredService(serviceType);

            Assert.NotNull(service);
        }

        public static IEnumerable<object[]> GetRequiredServiceData =>
        [
            [typeof(ISecurityConfigurationProvider)],
            [typeof(IPasswordValidationProvider)],

            [typeof(IMapProvider)],

            [typeof(IValidationProvider)],

            [typeof(ISymmetricEncryptionProviderFactory)],
            [typeof(IAsymmetricEncryptionProviderFactory)],
            [typeof(IHashProviderFactory)],

            [typeof(IAuthService)],
            [typeof(IAccountService)],
            [typeof(IUserService)],
            [typeof(IRegistrationService)],
            [typeof(ISecurityService)],

            // Repos are registered as generics. Only need to test each one once.
            [typeof(IRepo<AccountEntity>)],
            [typeof(IRepoExtendedGet<AccountEntity>)],
            [typeof(IReadonlyRepo<AccountEntity>)],

            [typeof(IUnitOfWorkProvider)]
        ];

        private object? GetRequiredService(Type serviceType)
        {
            var methodInfo = ServiceFactory.GetType()
                .GetMethod(nameof(ServiceFactory.GetRequiredService));

            return methodInfo?.MakeGenericMethod(serviceType)
                .Invoke(ServiceFactory, null);
        }

        [Fact]
        public void ServiceFactoryBase_DisposesServiceProviderCorrectly()
        {
            var mockServiceFactory = ServiceFactory;

            mockServiceFactory.Dispose();
            var ex = Record.Exception(() => mockServiceFactory.GetRequiredService<ILogger>());

            Assert.NotNull(ex);
            Assert.IsType<ObjectDisposedException>(ex);
        }
    }
}
