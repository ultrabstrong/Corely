using Corely.IAM.Permissions.Models;
using Corely.UnitTests.Mappers.AutoMapper;

namespace Corely.UnitTests.Permissions.Mappers;
public class CreatePermissionRequestProfileTests
    : ProfileDelegateTestsBase
{
    private class Delegate : ProfileTestsBase<CreatePermissionRequest, Permission>;
    protected override ProfileTestsBase GetDelegate() => new Delegate();
}
