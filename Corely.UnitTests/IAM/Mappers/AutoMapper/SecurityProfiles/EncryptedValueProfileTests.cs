using AutoFixture;
using Corely.Security.Encryption;
using Corely.Security.Encryption.Models;
using Corely.Security.Encryption.Providers;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.SecurityProfiles
{
    public class EncryptedValueProfileTests
        : BidirectionalProfileTestsBase<SymmetricEncryptedValue, string>
    {
        protected override string GetDestination()
            => $"{SymmetricEncryptionConstants.AES_CODE}:{new Fixture().Create<string>()}";
        protected override object[] GetSourceParams()
            => [Mock.Of<ISymmetricEncryptionProvider>()];
    }
}
