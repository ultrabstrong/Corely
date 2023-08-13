using Corely.Shared.Extensions;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Shared.Extensions
{
    public class Base64StringExtensionsTests
    {
        [Theory, ClassData(typeof(NullAndEmpty))]
        public void Base64Encode_ShouldReturnEmptyString_WhenStringIsNullOrEmpty(string s)
        {
            Assert.Equal(string.Empty, s.Base64Encode());
        }

        [Theory, ClassData(typeof(NullAndEmpty))]
        public void Base64Decode_ShouldReturnEmptyString_WhenStringIsNullOrEmpty(string s)
        {
            Assert.Equal(string.Empty, s.Base64Decode());
        }

        [Theory]
        [InlineData("test")]
        [InlineData("test string with spaces")]
        [InlineData("test string with spaces and special characters !@#$%^&*()_+")]
        public void Base64Encode_ShouldBase64DecodeToOriginalString(string s)
        {
            Assert.Equal(s, s.Base64Encode().Base64Decode());
        }

    }
}
