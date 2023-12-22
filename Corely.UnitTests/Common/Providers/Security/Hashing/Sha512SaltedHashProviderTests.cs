﻿using Corely.Common.Providers.Security.Hashing;

namespace Corely.UnitTests.Common.Providers.Security.Hashing
{
    public class Sha512SaltedHashProviderTests : SaltedHashProviderTests
    {
        private readonly Sha512SaltedHashProvider _sha512SaltedHashProvider = new();

        protected override IHashProvider HashProvider => _sha512SaltedHashProvider;

        public override void HashTypeCode_ShouldReturnCorrectCode_ForImplementation()
        {
            Assert.Equal(HashProviderConstants.SALTED_SHA512, _sha512SaltedHashProvider.HashTypeCode);
        }
    }
}
