using Corely.IAM.Groups.Entities;
using Corely.IAM.Groups.Models;
using Corely.UnitTests.Mappers.AutoMapper;

namespace Corely.UnitTests.Groups.Mappers;

public class GroupProfileTests
    : BidirectionalProfileDelegateTestsBase
{
    private class Delegate : BidirectionalProfileTestsBase<Group, GroupEntity>;

    protected override BidirectionalProfileTestsBase GetDelegate() => new Delegate();
}
