using ArgusBot.DAL.Models;
using System.Threading.Tasks;

namespace ArgusBot.DAL.Repositories.Interfaces
{
    public interface IGroupsRepositoryInterface
    {
        public Task AddGroupAsync(Group group);

        public Task DeleteGroupAsync(int groupId);

        public Task<Group> GetGroupByIdAsync(int groupId);

        public Task AddAdminToGroupAsync(int groupId, int userId);

        public Task DeleteAdminFromGroupAsync(int groupId, int userId);

        Task SaveChangesAsync();
    }
}
