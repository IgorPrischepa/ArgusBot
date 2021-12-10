using ArgusBot.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArgusBot.DAL.Repositories.Interfaces
{
    public interface ICheckListRepository
    {
        Task<ICollection<Check>> GetAllCheckListsAsync();

        Task<Check> GetItemByUserAndGroupIdAsync(long userId, long groupId);

        Task<IEnumerable<Check>> GetCheckByStatusAsync(StatusTypes status, int countChunk, int skippedCount);

        Task CreateNewAsync(Check item);

        Task UpdateAsync(Check item);

        Task DeleteAsync(long userId, long groupId);

        Task SaveAsync();
    }
}
