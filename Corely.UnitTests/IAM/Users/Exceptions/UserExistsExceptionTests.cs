using Corely.IAM.Users.Exceptions;

namespace Corely.UnitTests.IAM.Users.Exceptions
{
    public class UserExistsExceptionTests
        : ExceptionTestsBase<UserExistsException>
    {
        [Fact]
        public void Properties_ShouldInitAndBeAccessible()
        {
            var userExistsException = new UserExistsException()
            {
                UsernameExists = true,
                EmailExists = true,
            };

            Assert.True(userExistsException.UsernameExists);
            Assert.True(userExistsException.EmailExists);
        }
    }
}
