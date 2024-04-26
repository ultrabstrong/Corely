using Corely.IAM.Exceptions;

namespace Corely.UnitTests.IAM.Exceptions
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
