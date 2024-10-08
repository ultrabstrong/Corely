using AutoFixture;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Security.Entities;
using Corely.IAM.Security.Models;
using Corely.IAM.Users.Entities;
using Corely.Security.Encryption;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.SecurityProfiles
{
    public class SymmetricKeyProfileTests
    {
        public class ToSymmetricKeyEntity
            : BidirectionalProfileTestsBase<SymmetricKey, SymmetricKeyEntity>
        {
            protected override SymmetricKeyEntity ApplyDestinatonModifications(SymmetricKeyEntity destination)
            {
                destination.Key = $"{SymmetricEncryptionConstants.AES_CODE}:{new Fixture().Create<string>()}";
                return destination;
            }
        }

        public class ToAccountSymmetricKeyEntity
        : BidirectionalProfileTestsBase<SymmetricKey, AccountSymmetricKeyEntity>
        {
            protected override AccountSymmetricKeyEntity ApplyDestinatonModifications(AccountSymmetricKeyEntity destination)
            {
                destination.Key = $"{SymmetricEncryptionConstants.AES_CODE}:{new Fixture().Create<string>()}";
                return destination;
            }
        }

        public class ToUserSymmetricKeyEntity
            : BidirectionalProfileTestsBase<SymmetricKey, UserSymmetricKeyEntity>
        {
            protected override UserSymmetricKeyEntity ApplyDestinatonModifications(UserSymmetricKeyEntity destination)
            {
                destination.Key = $"{SymmetricEncryptionConstants.AES_CODE}:{new Fixture().Create<string>()}";
                return destination;
            }
        }
    }
}
