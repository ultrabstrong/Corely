﻿using AutoMapper;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Accounts.Models;

namespace Corely.IAM.Mappers.AutoMapper.AccountProfiles
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
