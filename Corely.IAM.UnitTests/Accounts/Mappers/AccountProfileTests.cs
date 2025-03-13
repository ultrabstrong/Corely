using Corely.IAM.Accounts.Entities;
using Corely.IAM.Accounts.Models;
using Corely.UnitTests.Mappers.AutoMapper;

namespace Corely.UnitTests.Accounts.Mappers;

public class AccountProfileTests
    : BidirectionalProfileDelegateTestsBase
{
    private class Delegate : BidirectionalProfileTestsBase<Account, AccountEntity>;

    protected override BidirectionalProfileTestsBase GetDelegate() => new Delegate();
}
