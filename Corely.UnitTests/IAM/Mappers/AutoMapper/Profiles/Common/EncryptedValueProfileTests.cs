using AutoFixture;
using Corely.Security.Encryption;
using Corely.Security.Encryption.Models;
using Corely.Security.Encryption.Providers;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.Profiles.Common
{
    public class EncryptedValueProfileTests
        : BidirectionalProfileTestsBase<EncryptedValue, string>
    {
        protected override string GetDestination()
            => $"{EncryptionConstants.AES_CODE}:{new Fixture().Create<string>()}";
        protected override object[] GetSourceParams()
            => [Mock.Of<IEncryptionProvider>()];
    }
}
