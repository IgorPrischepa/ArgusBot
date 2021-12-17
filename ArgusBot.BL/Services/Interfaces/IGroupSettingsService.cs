using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgusBot.BL.Services.Interfaces
{
    public interface IGroupSettingsService
    {
        Task<bool> isCapthcaEnabledAsync(long groupId);
    }
}
