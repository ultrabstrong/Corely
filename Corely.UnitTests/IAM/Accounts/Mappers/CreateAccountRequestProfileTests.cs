using Corely.IAM.Accounts.Models;
using Corely.UnitTests.IAM.Mappers.AutoMapper;

namespace Corely.UnitTests.IAM.Accounts.Mappers;

public class CreateAccountRequestProfileTests
    : ProfileDelegateTestsBase
{
    private class Delegate : ProfileTestsBase<CreateAccountRequest, Account>;

    protected override ProfileTestsBase GetDelegate() => new Delegate();
}
