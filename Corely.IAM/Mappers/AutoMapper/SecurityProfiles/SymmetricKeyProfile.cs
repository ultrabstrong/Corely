﻿using AutoMapper;
using Corely.IAM.Security.Entities;
using Corely.IAM.Security.Models;

namespace Corely.IAM.Mappers.AutoMapper.SecurityProfiles
{
    public class SymmetricKeyProfile : Profile
    {
        public SymmetricKeyProfile()
        {
            CreateMap<SymmetricKey, AccountSymmetricKeyEntity>()
                .ForMember(m => m.AccountId, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
