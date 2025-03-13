using Corely.IAM.Roles.Entities;
using Corely.IAM.Roles.Models;
using Corely.UnitTests.Mappers.AutoMapper;

namespace Corely.UnitTests.Roles.Mappers;
public class RoleProfileTests
    : BidirectionalProfileDelegateTestsBase
{
    private class Delegate : BidirectionalProfileTestsBase<Role, RoleEntity>;
    protected override BidirectionalProfileTestsBase GetDelegate() => new Delegate();
}
