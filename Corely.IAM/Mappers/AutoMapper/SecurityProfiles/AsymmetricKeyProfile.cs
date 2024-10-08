using AutoMapper;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Security.Entities;
using Corely.IAM.Security.Models;
using Corely.IAM.Users.Entities;

namespace Corely.IAM.Mappers.AutoMapper.SecurityProfiles
{
    public class AsymmetricKeyProfile : Profile
    {
        public AsymmetricKeyProfile()
        {
            CreateMap<AsymmetricKey, AsymmetricKeyEntity>()
                .ReverseMap();

            CreateMap<AsymmetricKey, AccountAsymmetricKeyEntity>()
                .ForMember(m => m.AccountId, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<AsymmetricKey, UserAsymmetricKeyEntity>()
                .ForMember(m => m.UserId, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
