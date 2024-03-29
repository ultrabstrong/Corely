﻿using Corely.Common.Providers.Security.Keys;
using Corely.UnitTests.ClassData;
using System.Security.Cryptography;

namespace Corely.UnitTests.Common.Providers.Security.Keys
{
    public class AesKeyProviderTests
    {
        private readonly AesKeyProvider _aesKeyProvider = new();

        [Fact]
        public void GetKey_ShouldReturnValidKeyKey()
        {
            var key = _aesKeyProvider.CreateKey();
            using (Aes aes = Aes.Create())
            {
                try
                {
                    aes.Key = Convert.FromBase64String(key);
                }
                catch (Exception)
                {
                    Assert.Fail("Aes key invalid");
                }
            }
        }

        [Fact]
        public void IsKeyValid_ShouldReturnTrue_WithKeyFromCreateKey()
        {
            var key = _aesKeyProvider.CreateKey();
            Assert.True(_aesKeyProvider.IsKeyValid(key));
        }

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void IsKeyValid_ShouldReturnFalse_WithNullOrWhitespaceKey(string key)
        {
            Assert.False(_aesKeyProvider.IsKeyValid(key));
        }

        [Fact]
        public void IsKeyValid_ShouldReturnTrueForValidKey()
        {
            var key = _aesKeyProvider.CreateKey();
            var isValid = _aesKeyProvider.IsKeyValid(key);
            Assert.True(isValid);
        }

        [Fact]
        public void IsKeyValid_ShouldReturnFalseForInvalidKey()
        {
            var isValid = _aesKeyProvider.IsKeyValid("asdf");
            Assert.False(isValid);
        }
    }
}
