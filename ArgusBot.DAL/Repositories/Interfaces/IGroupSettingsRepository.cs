﻿using System.Threading.Tasks;

using ArgusBot.DAL.Models;

namespace ArgusBot.DAL.Repositories.Interfaces
{
    public interface IGroupSettingsRepository
    {
        Task<bool> UpdateSettingsGroupAsync(GroupSettings groupSettings);

        Task<GroupSettings> GetSettingAsync(long groupId);

        Task AddSettingsAsync(GroupSettings groupSetttings);
    }
}
