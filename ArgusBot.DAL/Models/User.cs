using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArgusBot.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserGuid { get; set; }

        [MaxLength(40)]
        public string Login { get; set; }

        public string NormalizedLogin { get; set; }

        public string Password { get; set; }

        public string TelegramId { get; set; }
    }
}
