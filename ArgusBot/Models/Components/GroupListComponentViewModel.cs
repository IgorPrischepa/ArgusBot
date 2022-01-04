using ArgusBot.BL.DTO;
using System.Collections.Generic;

namespace ArgusBot.Models.Components
{
    public class GroupListComponentViewModel
    {
        public IEnumerable<GroupDTO> Groups { get; set; }
        public bool NeedToAttachTelegramAccount { get; set; }
    }
}
