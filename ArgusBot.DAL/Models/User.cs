using System;

namespace ArgusBot.Models
{
    public class User
    {
        public Guid UserGuid { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string TelegramAccount { get; set; }
    }
}
