using AutoFixture;
using Corely.Common.Models.Security;
using Corely.Common.Providers.Security.Encryption;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper.Profiles.Common
{
    public class EncryptedValueProfileTests
        : BidirectionalAutoMapperTestsBase<EncryptedValue, string>
    {
        protected override string GetDestination()
            => $"{EncryptionProviderConstants.AES_CODE}:{new Fixture().Create<string>()}";
        protected override object[] GetSourceParams()
            => [Mock.Of<IEncryptionProvider>()];
    }
}
