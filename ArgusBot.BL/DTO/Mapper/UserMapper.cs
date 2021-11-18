using ArgusBot.DAL.Models;
using AutoMapper;

namespace ArgusBot.BL.DTO.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<ProfileDTO, User>().ForAllMembers(c => c.MapFrom<ProfileDTOToUserResolver>());
            CreateMap<User, ProfileDTO>().ForAllMembers(c => c.MapFrom<UserToProfileDTOResolver>());
        }
    }
}
