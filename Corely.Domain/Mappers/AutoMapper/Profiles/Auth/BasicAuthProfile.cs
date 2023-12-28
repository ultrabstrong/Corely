﻿using AutoMapper;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Models.Auth;

namespace Corely.Domain.Mappers.AutoMapper.Profiles.Auth
{
    public class BasicAuthProfile : Profile
    {
        public BasicAuthProfile()
        {
            CreateMap<BasicAuth, BasicAuthEntity>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password.Secret));
        }
    }
}