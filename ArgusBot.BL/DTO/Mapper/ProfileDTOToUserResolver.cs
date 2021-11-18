using ArgusBot.BL.Services;
using ArgusBot.DAL.Models;
using AutoMapper;
using System;

namespace ArgusBot.BL.DTO.Mapper
{
    public class ProfileDTOToUserResolver : IValueResolver<ProfileDTO, User, object>
    {
        public object Resolve(ProfileDTO source, User destination, object destMember, ResolutionContext context)
        {
            if (source.VerifyNotNull("Source for mapping cannot be null!"))
            {
                destination.Login = source.Login ?? "";
                destination.TelegramId = source.TelegramId ?? "";
                destination.UserGuid = source.UserGuid;
                return destination;
            }
            throw new InvalidOperationException("Cannot map ProfileDTO to User instance!");
        }
    }
}
