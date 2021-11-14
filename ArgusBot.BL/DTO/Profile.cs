using System;

namespace ArgusBot.BL.DTO
{
    public class Profile
    {
        public Guid UserGuid { get; set; }

        public string Login { get; set; }

        public string TelegramId { get; set; }
    }
}
