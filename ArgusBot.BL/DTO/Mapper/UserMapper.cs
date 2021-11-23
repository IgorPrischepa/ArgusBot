﻿using ArgusBot.DAL.Models;
using AutoMapper;

namespace ArgusBot.BL.DTO.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<ProfileDTO, User>().ForMember(d => d.Login, map => map.MapFrom(src => src.Login ?? ""))
                                         .ForMember(d => d.TelegramId, map => map.MapFrom(src => src.TelegramId ?? ""))
                                         .ForMember(d => d.UserGuid, map => map.MapFrom(src => src.UserGuid))
                                         .ForMember(d => d.NormalizedLogin, map => map.MapFrom(src => src.Login.ToLower() ?? ""))
                                         .ForMember(d => d.NormalizedTelegramLogin, map => map.MapFrom(src => src.Login.ToLower() ?? ""));
            CreateMap<User, ProfileDTO>().ForMember(d => d.Login, map => map.MapFrom(src => src.Login ?? ""))
                                         .ForMember(d => d.TelegramId, map => map.MapFrom(src => src.TelegramId ?? ""))
                                         .ForMember(d => d.UserGuid, map => map.MapFrom(src => src.UserGuid));
        }
    }
}