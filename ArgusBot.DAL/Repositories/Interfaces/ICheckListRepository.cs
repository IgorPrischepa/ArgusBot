using ArgusBot.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArgusBot.DAL.Repositories.Interfaces
{
    public interface ICheckListRepository
    {
        Task<ICollection<Check>> GetAllCheckListsAsync();

        Task<Check> GetItemByUserIdAsync(long userId);

        Task<List<Check>> GetCheckByStatusAsync(StatusTypes status);

        Task CreateNewAsync(Check item);

        Task UpdateAsync(Check item);

        Task DeleteAsync(long userId);

        Task SaveAsync();
    }
}
