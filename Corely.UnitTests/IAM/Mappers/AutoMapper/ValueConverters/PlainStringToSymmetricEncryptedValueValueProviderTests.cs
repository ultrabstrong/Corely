using AutoFixture;
using Corely.IAM;
using Corely.IAM.Mappers.AutoMapper.ValueConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.ValueConverters
{
    public class PlainStringToSymmetricEncryptedValueValueProviderTests
    {
        private readonly Fixture _fixture = new();
        private readonly ISecurityConfigurationProvider _securityConfigurationProviderMock;
        private readonly PlainStringToSymmetricEncryptedValueValueProvider _converter;

        public PlainStringToSymmetricEncryptedValueValueProviderTests()
        {
            var serviceFactory = new ServiceFactory();
            _converter = new(serviceFactory.GetRequiredService<ISecurityConfigurationProvider>());
            _securityConfigurationProviderMock = serviceFactory.GetRequiredService<ISecurityConfigurationProvider>();
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenSecurityConfigurationProviderIsNull()
        {
            static PlainStringToSymmetricEncryptedValueValueProvider act() => new(null);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }


        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenSystemSymmetricKeyIsNull()
        {
            static PlainStringToSymmetricEncryptedValueValueProvider act() => new(
                Mock.Of<ISecurityConfigurationProvider>());

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }
    }
}
