using Corely.IAM;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Accounts.Services;
using Corely.IAM.Mappers;
using Corely.IAM.Repos;
using Corely.IAM.Security.Services;
using Corely.Security.Keys.Symmetric;
using Corely.Security.KeyStore.Symmetric;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corely.UnitTests.IAM.Security.Services
{
    public class SecurityServiceTests
    {
        private readonly ISecurityConfigurationProvider _securityConfigurationProviderMock;

        public SecurityServiceTests()
        {
            var serviceFactory = new ServiceFactory();
            _securityConfigurationProviderMock = serviceFactory.GetRequiredService<ISecurityConfigurationProvider>();
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenSymmetricKeyProviderIsNull()
        {
            SecurityService act() => new(
                null!,
                _securityConfigurationProviderMock);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenSecurityConfigurationProviderIsNull()
        {
            static SecurityService act() => new(
                Mock.Of<ISymmetricKeyProvider>(),
                null);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenSystemSymmetricKeyIsNull()
        {
            static SecurityService act() => new(
                Mock.Of<ISymmetricKeyProvider>(),
                Mock.Of<ISecurityConfigurationProvider>());

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }
    }
}
