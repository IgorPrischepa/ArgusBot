using ArgusBot.BL.Services;
using ArgusBot.DAL.Models;
using AutoMapper;
using System;

namespace ArgusBot.BL.DTO.Mapper
{
    class UserToProfileDTOResolver : IValueResolver<User, ProfileDTO, object>
    {
        public object Resolve(User source, ProfileDTO destination, object destMember, ResolutionContext context)
        {
            if (source.VerifyNotNull("Source for mapping cannot be null!"))
            {
                destination.Login = source.Login;
                destination.TelegramId = string.IsNullOrEmpty(source.TelegramId) ? source.TelegramId : "";
                destination.UserGuid = source.UserGuid;
                return destination;
            }
            throw new ArgumentNullException("Cannotmap User to ProfileDTO instance!");
        }
    }
}
