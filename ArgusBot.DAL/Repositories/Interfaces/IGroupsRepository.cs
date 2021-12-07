using ArgusBot.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArgusBot.DAL.Repositories.Interfaces
{
    public interface IGroupsRepository
    {
        public Task AddGroupAsync(Group group);

        public Task DeleteGroupAsync(int groupId);

        public Task<Group> GetGroupByIdAsync(int groupId);

        public Task AddAdminToGroupAsync(int groupId, long userId);

        public Task DeleteAdminFromGroupAsync(int groupId, long userId);

        public Task<List<Group>> GetAllGroupAsync();

        public Task<List<Group>> GetAllGroupWithAdminsAsync();

        Task SaveChangesAsync();
    }
}
