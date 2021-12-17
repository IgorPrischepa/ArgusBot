using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ArgusBot.BL.Services.Interfaces;
using ArgusBot.DAL.Models;
using ArgusBot.DAL.Repositories.Interfaces;

namespace ArgusBot.BL.Services.Implementation
{
    public class GroupSettingsService : IGroupSettingsService
    {
        private readonly IGroupSettingsRepository _groupSettings;
        private readonly IGroupService _groups;


        public GroupSettingsService(IGroupSettingsRepository groupSettings, IGroupService group)
        {
            _groupSettings = groupSettings;
            _groups = group;
        }

        public async Task<bool> isCapthcaEnabledAsync(long groupId)
        {
            if (groupId == 0) throw new ArgumentException("Group id can't be equals to zero.");

            Group group = await _groups.GetGroupByIdAsync(groupId);

            if (group is not null && group.Settings is not null)
            {
                return group.Settings.IsCpatchaEnabled;
            }

            return false;
        }
    }
}
