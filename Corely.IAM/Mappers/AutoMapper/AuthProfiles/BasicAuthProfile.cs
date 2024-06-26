﻿using AutoMapper;
using Corely.IAM.Auth.Entities;
using Corely.IAM.Auth.Models;

namespace Corely.IAM.Mappers.AutoMapper.AuthProfiles
{
    internal sealed class BasicAuthProfile : Profile
    {
        public BasicAuthProfile()
        {
            CreateMap<BasicAuth, BasicAuthEntity>(MemberList.Source)
                .ReverseMap();
        }
    }
}
