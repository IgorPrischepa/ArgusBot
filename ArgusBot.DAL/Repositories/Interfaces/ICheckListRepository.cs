using ArgusBot.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArgusBot.DAL.Repositories.Interfaces
{
    public interface ICheckListRepository
    {
        Task<ICollection<Check>> GetAllCheckListsAsync();

        Task<Check> GetItemByUserIdAsync(int userId);

        Task CreateNewAsync(Check item);

        Task UpdateAsync(Check item);

        Task DeleteAsync(int userId);

        Task SaveAsync();
    }
}
