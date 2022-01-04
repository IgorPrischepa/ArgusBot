using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgusBot.BL.DTO
{
    public class GroupSettingsDTO
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public bool IsCpatchaEnabled { get; set; }
    }
}
