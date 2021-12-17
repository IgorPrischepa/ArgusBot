using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgusBot.DAL.Models
{
    public class GroupSettings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public long TelegramChatId { get; set; }

        public bool IsCpatchaEnabled { get; set; } = true;

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
