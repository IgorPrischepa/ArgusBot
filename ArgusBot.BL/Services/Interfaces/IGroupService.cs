using ArgusBot.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArgusBot.BL.Services.Interfaces
{
    public interface IGroupService
    {
        public Task AddGroupAsync(long groupId, string groupName);

        public Task RemoveGroupAsync(long groupId);

        public Task AddAdminToTelegramGroupAsync(long groupId, long userId);

        public Task RemoveAdminFromTelegramGroupAsync(long groupId, long userId);
        public IEnumerable<Group> GetGroupsForCurrentAdmin(long userId);
        public Task<List<Group>> GetAllGroupsAsync();

        public Task<List<Group>> GetGroupsWithAdminsAsync();
        public Task RemoveGroupWithAdmins(long groupId);
        public Task<bool> Exists(long groupId);
    }
}
