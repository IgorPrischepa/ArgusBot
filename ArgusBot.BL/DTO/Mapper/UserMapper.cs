using ArgusBot.DAL.Models;

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

            CreateMap<Group, GroupDTO>().ForMember(g => g.Id, map => map.MapFrom(src => src.GroupId))
                                        .ForMember(g => g.Name, map => map.MapFrom(src => src.GroupName))
                                        .ForMember(n => n.Settings, map => map.MapFrom(src => src.Settings));

            CreateMap<GroupSettings, GroupSettingsDTO>().ForMember(d => d.Id, map => map.MapFrom(src => src.Id))
                                          .ForMember(d => d.GroupId, map => map.MapFrom(src => src.TelegramChatId))
                                          .ForMember(d => d.IsCpatchaEnabled, map => map.MapFrom(src => src.IsCpatchaEnabled));
        }
    }
}
