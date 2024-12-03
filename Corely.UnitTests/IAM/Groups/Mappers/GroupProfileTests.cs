using Corely.IAM.Groups.Entities;
using Corely.IAM.Groups.Models;
using Corely.UnitTests.IAM.Mappers.AutoMapper;

namespace Corely.UnitTests.IAM.Groups.Mappers;

public class GroupProfileTests
    : BidirectionalProfileDelegateTestsBase
{
    private class Delegate : BidirectionalProfileTestsBase<Group, GroupEntity>;

    protected override BidirectionalProfileTestsBase GetDelegate() => new Delegate();
}
