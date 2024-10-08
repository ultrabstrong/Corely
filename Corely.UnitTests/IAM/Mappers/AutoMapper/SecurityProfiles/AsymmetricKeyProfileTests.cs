using AutoFixture;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Security.Entities;
using Corely.IAM.Security.Models;
using Corely.IAM.Users.Entities;
using Corely.Security.Encryption;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.SecurityProfiles
{
    public class AsymmetricKeyProfileTests
    {
        public class ToAsymmetricKeyEntity
            : BidirectionalProfileTestsBase<AsymmetricKey, AsymmetricKeyEntity>
        {
            protected override AsymmetricKeyEntity ApplyDestinatonModifications(AsymmetricKeyEntity destination)
            {
                destination.PrivateKey = $"{AsymmetricEncryptionConstants.RSA_CODE}:{new Fixture().Create<string>()}";
                return destination;
            }
        }

        public class ToAccountAsymmetricKeyEntity
            : BidirectionalProfileTestsBase<AsymmetricKey, AccountAsymmetricKeyEntity>
        {
            protected override AccountAsymmetricKeyEntity ApplyDestinatonModifications(AccountAsymmetricKeyEntity destination)
            {
                destination.PrivateKey = $"{AsymmetricEncryptionConstants.RSA_CODE}:{new Fixture().Create<string>()}";
                return destination;
            }
        }

        public class ToUserAsymmetricKeyEntity
            : BidirectionalProfileTestsBase<AsymmetricKey, UserAsymmetricKeyEntity>
        {
            protected override UserAsymmetricKeyEntity ApplyDestinatonModifications(UserAsymmetricKeyEntity destination)
            {
                destination.PrivateKey = $"{AsymmetricEncryptionConstants.RSA_CODE}:{new Fixture().Create<string>()}";
                return destination;
            }
        }
    }
}
