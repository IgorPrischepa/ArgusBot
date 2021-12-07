using ArgusBot.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArgusBot.DAL.Repositories.Interfaces
{
    public interface IGroupsRepository
    {
        public Task AddGroupAsync(Group group);

        public Task DeleteGroupAsync(long groupId);

        public Task<Group> GetGroupByIdAsync(long groupId);

        public Task AddAdminToGroupAsync(long groupId, long userId);

        public Task DeleteAdminFromGroupAsync(long groupId, long userId);

        public Task<List<Group>> GetAllGroupAsync();

        public Task<List<Group>> GetAllGroupWithAdminsAsync();

        Task SaveChangesAsync();
    }
}
