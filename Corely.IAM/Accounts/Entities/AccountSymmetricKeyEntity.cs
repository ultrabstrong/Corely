﻿using Corely.IAM.Entities;
using Corely.IAM.Security.Entities;

namespace Corely.IAM.Accounts.Entities
{
    public class AccountSymmetricKeyEntity : SymmetricKeyEntity, IHasIdPk
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
    }
}
