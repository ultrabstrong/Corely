using AutoMapper;
using Corely.IAM.Accounts.Models;

namespace Corely.IAM.Mappers.AutoMapper.AccountProfiles
{
    public class CreateAccountRequestProfile : Profile
    {
        public CreateAccountRequestProfile()
        {
            CreateMap<CreateAccountRequest, Account>(MemberList.Source);
        }
    }
}
