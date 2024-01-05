using Corely.Common.Providers.Security.Hashing;
using Corely.Domain.Models.Auth;
using Corely.Domain.Models.Users;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper.Profiles.Users
{
    public class CreateUserRequestTests
    {
        public class ToUserTests
            : AutoMapperTestsBase<CreateUserRequest, User>
        {
        }

        public class ToBasicAuthTests
            : AutoMapperTestsBase<CreateUserRequest, BasicAuth>
        {
            protected override CreateUserRequest ApplySourceModifications(CreateUserRequest source)
            {
                return new CreateUserRequest(
                    source.Username,
                    source.Email,
                    $"{HashProviderConstants.SALTED_SHA256_CODE}:{source.Password}");
            }
        }
    }
}
