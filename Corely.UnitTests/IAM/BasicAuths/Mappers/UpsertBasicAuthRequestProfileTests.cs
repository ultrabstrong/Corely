using Corely.IAM.BasicAuths.Models;
using Corely.UnitTests.IAM.Mappers.AutoMapper;

namespace Corely.UnitTests.IAM.BasicAuths.Mappers;

public class UpsertBasicAuthRequestProfileTests
    : ProfileDelegateTestsBase
{
    private class Delegate : ProfileTestsBase<UpsertBasicAuthRequest, BasicAuth>;

    protected override ProfileTestsBase GetDelegate() => new Delegate();
}
