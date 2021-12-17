using ArgusBot.BL.Services.Interfaces;
using ArgusBot.DAL.Models;
using ArgusBot.DAL.Repositories.Interfaces;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgusBot.BL.Services.Implementation
{
    public class GroupService : IGroupService
    {
        private readonly IGroupsRepository _groupRepository;
        private readonly IGroupSettingsRepository _groupSettingsRepository;

        public GroupService(IGroupsRepository repository, IGroupSettingsRepository groupSettings)
        {
            _groupRepository = repository;
            _groupSettingsRepository = groupSettings;
        }

        public async Task AddAdminToTelegramGroupAsync(long groupId, long userId)
        {
            await _groupRepository.AddAdminToGroupAsync(groupId, userId);
        }

        public async Task AddGroupAsync(long groupId, string groupName)
        {
            Group checkingGroup = await _groupRepository.GetGroupByIdWithSettingsAsync(groupId);

            if (checkingGroup is null)
            {
                Group group = new()
                {
                    GroupId = groupId,
                    GroupName = groupName
                };

                Group createdGroup = await _groupRepository.AddGroupAsync(group);

                if (createdGroup is not null)
                {
                    GroupSettings groupSettings = new GroupSettings()
                    {
                        TelegramChatId = groupId,
                        Group = createdGroup
                    };

                    await _groupSettingsRepository.AddSettingsAsync(groupSettings);
                }
            }
        }

        public async Task<bool> Exists(long groupId)
        {
            Group group = await _groupRepository.GetGroupByIdWithSettingsAsync(groupId);
            if (group != null)
            {
                return true;
            }
            return false;
        }

        public Task<List<Group>> GetAllGroupsAsync()
        {
            return _groupRepository.GetAllGroupAsync();
        }

        public async Task<Group> GetGroupByIdAsync(long groupId)
        {
            return await _groupRepository.GetGroupByIdWithSettingsAsync(groupId);
        }

        public IEnumerable<Group> GetGroupsForCurrentAdmin(long userId)
        {
            var groups = _groupRepository.GetGroupsForUser(userId);
            if (groups.Any())
            {
                return groups;
            }
            return null;
        }

        public async Task<List<Group>> GetGroupsWithAdminsAsync()
        {
            return await _groupRepository.GetAllGroupWithAdminsAsync();
        }

        public async Task RemoveAdminFromTelegramGroupAsync(long groupId, long userId)
        {
            await _groupRepository.DeleteAdminFromGroupAsync(groupId, userId);
        }

        public async Task RemoveGroupAsync(long groupId)
        {
            await _groupRepository.DeleteGroupAsync(groupId);
        }

        public async Task RemoveGroupWithAdmins(long groupId)
        {
            await _groupRepository.DeleteGroupWithAdmins(groupId);
        }
    }
}
