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

        public async Task AddAdminToTelegramGroupAsync(int groupId, int userId)
        {
            await groupRepository.AddAdminToGroupAsync(groupId, userId);
        }

        public async Task AddGroupAsync(int groupId, string groupName)
        {
            Group group = new Group
            {
                GroupId = groupId,
                GroupName = groupName
            };

            await groupRepository.AddGroupAsync(group);
        }

        public Task<List<Group>> GetAllGroupsAsync()
        {
            return groupRepository.GetAllGroupAsync();
        }

        public async Task<List<Group>> GetGroupsWithAdminsAsync()
        {
            return await groupRepository.GetAllGroupWithAdminsAsync();
        }

        public async Task RemoveAdminFromTelegramGroupAsync(int groupId, int userId)
        {
            await groupRepository.DeleteAdminFromGroupAsync(groupId, userId);
        }

        public async Task RemoveGroupAsync(int groupId)
        {
            await groupRepository.DeleteGroupAsync(groupId);
        }
    }
}
