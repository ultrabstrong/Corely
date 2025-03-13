using Corely.IAM.Users.Models;
using Corely.IAM.UnitTests.Mappers.AutoMapper;

namespace Corely.IAM.UnitTests.Users.Mappers;

public class CreateUserRequestTests
    : ProfileDelegateTestsBase
{
    private class Delegate : ProfileTestsBase<CreateUserRequest, User>;

    protected override ProfileTestsBase GetDelegate() => new Delegate();
}
