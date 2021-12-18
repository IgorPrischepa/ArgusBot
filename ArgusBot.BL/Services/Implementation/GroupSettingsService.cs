using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ArgusBot.BL.DTO;
using ArgusBot.BL.Services.Interfaces;
using ArgusBot.DAL.Models;
using ArgusBot.DAL.Repositories.Interfaces;

using AutoMapper;

namespace ArgusBot.BL.Services.Implementation
{
    public class GroupSettingsService : IGroupSettingsService
    {
        private readonly IGroupSettingsRepository _groupSettings;
        private readonly IGroupService _groups;
        private readonly IMapper _mapper;


        public GroupSettingsService(IGroupSettingsRepository groupSettings, IGroupService group, IMapper mapper)
        {
            _groupSettings = groupSettings;
            _groups = group;
            _mapper = mapper;
        }

        public async Task<GroupSettingsDTO> GetGroupSettingsAsync(long groupId)
        {
            if (groupId == 0) throw new ArgumentException("Group id can't be equals to zero.");

            GroupSettings groupSettings = await _groupSettings.GetSettingAsync(groupId);

            return _mapper.Map<GroupSettings, GroupSettingsDTO>(groupSettings);
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

        public async Task<bool> UpdateSettingsAsync(GroupSettingsDTO settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            GroupSettings groupSettings = await _groupSettings.GetSettingAsync(settings.GroupId);

            if (groupSettings is not null)
            {
                groupSettings.IsCpatchaEnabled = settings.IsCpatchaEnabled;

                return await _groupSettings.UpdateSettingsGroupAsync(groupSettings);
            }

            return false;
        }
    }
}
