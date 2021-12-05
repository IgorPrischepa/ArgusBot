using ArgusBot.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArgusBot.BL.Services.Interfaces
{
    public interface IGroupService
    {
        public Task AddGroupAsync(int groupId, string groupName);

        public Task RemoveGroupAsync(int groupId);

        public Task AddAdminToTelegramGroupAsync(int groupId, int userId);

        public Task RemoveAdminFromTelegramGroupAsync(int groupId, int userId);

        public Task<List<Group>> GetAllGroupsAsync();

        public Task<List<Group>> GetGroupsWithAdminsAsync();
    }
}
