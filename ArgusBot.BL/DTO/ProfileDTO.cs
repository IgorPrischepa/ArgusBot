using ArgusBot.BL.Services;
using System;

namespace ArgusBot.BL.DTO
{
    public class ProfileDTO
    {
        public Guid UserGuid { get; set; }

        public string Login { get; set; }

        public string TelegramId { get; set; }
    }
}
