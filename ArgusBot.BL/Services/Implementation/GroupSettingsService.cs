using System;
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
        private readonly IMapper _mapper;


        public GroupSettingsService(IGroupSettingsRepository groupSettings, IMapper mapper)
        {
            _groupSettings = groupSettings;
            _mapper = mapper;
        }

        public async Task<GroupSettingsDTO> GetGroupSettingsAsync(long groupId)
        {
            if (groupId == 0) throw new ArgumentException("Group id can't be equals to zero.");

            GroupSettings groupSettings = await _groupSettings.GetSettingAsync(groupId);

            return _mapper.Map<GroupSettings, GroupSettingsDTO>(groupSettings);
        }

        public async Task<bool> IsCapthcaEnabledAsync(long groupId)
        {
            if (groupId == 0) throw new ArgumentException("Group id can't be equals to zero.");

            GroupSettings groupSettings = await _groupSettings.GetSettingAsync(groupId);

            if (groupSettings is not null)
            {
                return groupSettings.IsCpatchaEnabled;
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
