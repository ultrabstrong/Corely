using AutoMapper;
using Corely.IAM.Entities.Accounts;
using Corely.IAM.Models.Accounts;

namespace Corely.IAM.Mappers.AutoMapper.Profiles.Accounts
{
    internal class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<Account, AccountEntity>(MemberList.Source)
                .ReverseMap();
        }
    }
}
