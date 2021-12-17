using System.Collections.Generic;

using ArgusBot.BL.DTO;

namespace ArgusBot.Models.Components
{
    public class GroupListComponentViewModel
    {
        public IEnumerable<GroupDTO> Groups { get; set; }
        public bool NeedToAttachTelegramAccount { get; set; }
    }
}
