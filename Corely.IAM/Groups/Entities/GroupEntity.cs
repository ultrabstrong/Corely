using Corely.DataAccess.Interfaces.Entities;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Users.Entities;

namespace Corely.IAM.Groups.Entities
{
    internal class GroupEntity : IHasIdPk, IHasCreatedUtc, IHasModifiedUtc
    {
        public int Id { get; set; }
        public string GroupName { get; set; } = null!;
        public int AccountId { get; set; }
        public virtual AccountEntity? Account { get; set; } = null!;
        public virtual ICollection<UserEntity>? Users { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
    }
}
