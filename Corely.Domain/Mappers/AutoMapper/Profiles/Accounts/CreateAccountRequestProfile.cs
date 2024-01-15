using AutoMapper;
using Corely.Domain.Models.Accounts;

namespace Corely.Domain.Mappers.AutoMapper.Profiles.Accounts
{
    public class CreateAccountRequestProfile : Profile
    {
        public CreateAccountRequestProfile()
        {
            CreateMap<CreateAccountRequest, Account>(MemberList.Source);
        }
    }
}
