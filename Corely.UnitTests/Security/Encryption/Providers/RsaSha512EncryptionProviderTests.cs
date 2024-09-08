﻿using Corely.Security.Encryption;
using Corely.Security.Encryption.Providers;

namespace Corely.UnitTests.Security.Encryption.Providers
{
    public class RsaSha512EncryptionProviderTests : AsymmetricEncryptionProviderGenericTests
    {
        private readonly RsaSha512EncryptionProvider _rsaEncryptionProvider = new();

        [Fact]
        public override void EncryptionTypeCode_ReturnsCorrectCode_ForImplementation()
        {
            Assert.Equal(AsymmetricEncryptionConstants.RSA_SHA512_CODE, _rsaEncryptionProvider.EncryptionTypeCode);
        }

        public override IAsymmetricEncryptionProvider GetEncryptionProvider()
        {
            return new RsaSha512EncryptionProvider();
        }
    }
}
