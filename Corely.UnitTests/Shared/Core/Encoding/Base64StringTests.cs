using Corely.Shared.Core.Encoding;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Core.Encoding
{
    public class Base64StringTests
    {
        [Theory, ClassData(typeof(NullAndEmpty))]
        public void Base64Encode_ShouldReturnEmptyString_WhenStringIsNullOrEmpty(string s)
        {
            Assert.Equal(string.Empty, Base64String.Base64Encode(s));
        }

        [Theory, ClassData(typeof(NullAndEmpty))]
        public void Base64Decode_ShouldReturnEmptyString_WhenStringIsNullOrEmpty(string s)
        {
            Assert.Equal(string.Empty, Base64String.Base64Decode(s));
        }

        [Theory]
        [InlineData("test")]
        [InlineData("test string with spaces")]
        [InlineData("test string with spaces and special characters !@#$%^&*()_+")]
        public void Base64Encode_ShouldBase64DecodeToOriginalString(string s)
        {
            Assert.Equal(s, Base64String.Base64Decode(Base64String.Base64Encode(s)));
        }

    }
}
