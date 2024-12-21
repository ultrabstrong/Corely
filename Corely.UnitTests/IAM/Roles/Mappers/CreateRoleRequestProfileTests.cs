using Corely.IAM.Roles.Models;
using Corely.UnitTests.IAM.Mappers.AutoMapper;

namespace Corely.UnitTests.IAM.Roles.Mappers;
public class CreateRoleRequestProfileTests
    : ProfileDelegateTestsBase
{
    private class Delegate : ProfileTestsBase<CreateRoleRequest, Role>;
    protected override ProfileTestsBase GetDelegate() => new Delegate();
}
