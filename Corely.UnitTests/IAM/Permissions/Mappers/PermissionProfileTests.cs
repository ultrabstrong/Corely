using Corely.IAM.Permissions.Entities;
using Corely.IAM.Permissions.Models;
using Corely.UnitTests.IAM.Mappers.AutoMapper;

namespace Corely.UnitTests.IAM.Permissions.Mappers;
public class PermissionProfileTests
    : BidirectionalProfileDelegateTestsBase
{
    private class Delegate : BidirectionalProfileTestsBase<Permission, PermissionEntity>;
    protected override BidirectionalProfileTestsBase GetDelegate() => new Delegate();
}
