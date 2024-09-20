﻿using AutoFixture;
using Corely.IAM.Security.Entities;
using Corely.IAM.Security.Models;
using Corely.Security.Encryption;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.SecurityProfiles
{
    public class AsymmetricKeyProfileTests
        : BidirectionalProfileTestsBase<AsymmetricKey, AccountAsymmetricKeyEntity>
    {
        protected override AccountAsymmetricKeyEntity ApplyDestinatonModifications(AccountAsymmetricKeyEntity destination)
        {
            destination.PrivateKey = $"{AsymmetricEncryptionConstants.RSA_SHA256_CODE}:{new Fixture().Create<string>()}";
            return destination;
        }
    }
}