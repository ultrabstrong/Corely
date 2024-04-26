using AutoMapper;
using Corely.IAM.Models.Accounts;

namespace Corely.IAM.Mappers.AutoMapper.Profiles.Accounts
{
    public class CreateAccountRequestProfile : Profile
    {
        public CreateAccountRequestProfile()
        {
            CreateMap<CreateAccountRequest, Account>(MemberList.Source);
        }
    }
}
