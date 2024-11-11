using Corely.DataAccess.Interfaces.Repos;
using Corely.DataAccess.Interfaces.UnitOfWork;
using Corely.IAM;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Accounts.Services;
using Corely.IAM.Auth.Services;
using Corely.IAM.Mappers;
using Corely.IAM.Security.Models;
using Corely.IAM.Security.Services;
using Corely.IAM.Services;
using Corely.IAM.Users.Services;
using Corely.IAM.Validators;
using Corely.Security.Encryption.Factories;
using Corely.Security.Hashing.Factories;
using Corely.Security.PasswordValidation.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Corely.UnitTests.IAM
{
    public abstract class ServiceFactoryGenericTests
    {
        protected abstract ServiceFactoryBase ServiceFactory { get; }

        [Theory, MemberData(nameof(GetRequiredServiceData))]
        public void ServiceFactoryBase_ProvidesService(Type serviceType)
        {
            var serviceCollection = new ServiceCollection();
            ServiceFactory.AddIAMServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var service = serviceProvider.GetRequiredService(serviceType);

            Assert.NotNull(service);
        }

        public static IEnumerable<object[]> GetRequiredServiceData =>
        [
            [typeof(ISecurityConfigurationProvider)],
            [typeof(IPasswordValidationProvider)],
            [typeof(IOptions<SecurityOptions>)],

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
    }
}
