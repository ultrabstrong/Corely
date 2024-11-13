﻿using Corely.DataAccess.Interfaces.Entities;
using Corely.IAM.Users.Entities;

namespace Corely.IAM.Groups.Entities
{
    internal class GroupEntity : IHasIdPk, IHasCreatedUtc, IHasModifiedUtc
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int AccountId { get; set; }
        public virtual ICollection<UserEntity>? Users { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
    }
}
