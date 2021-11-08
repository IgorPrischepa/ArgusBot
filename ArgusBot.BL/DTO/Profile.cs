using System;

namespace ArgusBot.BLL.DTO
{
    public class Profile
    {
        public Guid UserGuid { get; set; }

        public string Login { get; set; }

        public string TelegramId { get; set; }
    }
}
