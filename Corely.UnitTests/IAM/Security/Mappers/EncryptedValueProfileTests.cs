using AutoFixture;
using Corely.Security.Encryption;
using Corely.Security.Encryption.Models;
using Corely.Security.Encryption.Providers;
using Corely.UnitTests.IAM.Mappers.AutoMapper;

namespace Corely.UnitTests.IAM.Security.Mappers;

public class EncryptedValueProfileTests
    : BidirectionalProfileTestsBase<SymmetricEncryptedValue, string>
{
    protected override string GetDestination()
        => $"{SymmetricEncryptionConstants.AES_CODE}:{new Fixture().Create<string>()}";
    protected override object[] GetSourceParams()
        => [Mock.Of<ISymmetricEncryptionProvider>()];
}
