using Corely.IAM.Groups.Models;
using Corely.UnitTests.IAM.Mappers.AutoMapper;

namespace Corely.UnitTests.IAM.Groups.Mappers;

public class CreateGroupRequestProfileTests
    : ProfileDelegateTestsBase
{
    private class Delegate : ProfileTestsBase<CreateGroupRequest, Group>;
    protected override ProfileTestsBase GetDelegate() => new Delegate();
}
