using ArgusBot.DAL.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArgusBot.DAL.Repositories.Interfaces
{
    public interface IGroupsRepository
    {
        public Task<Group> AddGroupAsync(Group group);

        public Task DeleteGroupAsync(long groupId);

        public Task<Group> GetGroupByIdWithSettingsAsync(long groupId);

        public Task AddAdminToGroupAsync(long groupId, long userId);

        public Task DeleteAdminFromGroupAsync(long groupId, long userId);

        public Task<List<Group>> GetAllGroupAsync();

        public IEnumerable<Group> GetGroupsForUser(long userId);

        public Task<List<Group>> GetAllGroupWithAdminsAsync();

        public Task DeleteGroupWithAdmins(long groupId);

        Task SaveChangesAsync();
    }
}
