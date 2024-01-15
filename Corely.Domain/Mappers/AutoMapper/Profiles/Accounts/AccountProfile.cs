using AutoMapper;
using Corely.Domain.Entities.Accounts;
using Corely.Domain.Models.Accounts;

namespace Corely.Domain.Mappers.AutoMapper.Profiles.Accounts
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
