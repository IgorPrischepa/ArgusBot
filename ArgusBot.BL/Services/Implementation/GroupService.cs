using ArgusBot.BL.Services.Interfaces;
using ArgusBot.DAL.Models;
using ArgusBot.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArgusBot.BL.Services.Implementation
{
    public class GroupService : IGroupService
    {
        private readonly IGroupsRepository groupRepository;

        public GroupService(IGroupsRepository repository)
        {
            groupRepository = repository;
        }

        public async Task AddAdminToTelegramGroupAsync(long groupId, long userId)
        {
            await groupRepository.AddAdminToGroupAsync(groupId, userId);
        }

        public async Task AddGroupAsync(long groupId, string groupName)
        {
            Group checkingGroup = await groupRepository.GetGroupByIdAsync(groupId);
            if (checkingGroup is null)
            {
                Group group = new Group
                {
                    GroupId = groupId,
                    GroupName = groupName
                };
                await groupRepository.AddGroupAsync(group);
            }
        }

        public async Task<bool> Exists(long groupId)
        {
            Group group = await groupRepository.GetGroupByIdAsync(groupId);
            if (group != null)
            {
                return true;
            }
            return false;
        }

        public Task<List<Group>> GetAllGroupsAsync()
        {
            return groupRepository.GetAllGroupAsync();
        }

        public async Task<List<Group>> GetGroupsWithAdminsAsync()
        {
            return await groupRepository.GetAllGroupWithAdminsAsync();
        }

        public async Task RemoveAdminFromTelegramGroupAsync(long groupId, long userId)
        {
            await groupRepository.DeleteAdminFromGroupAsync(groupId, userId);
        }

        public async Task RemoveGroupAsync(long groupId)
        {
            await groupRepository.DeleteGroupAsync(groupId);
        }

        public async Task RemoveGroupWithAdmins(long groupId)
        {
            await groupRepository.DeleteGroupWithAdmins(groupId);
        }
    }
}
