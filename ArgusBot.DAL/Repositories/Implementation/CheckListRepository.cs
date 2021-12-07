using ArgusBot.DAL.Models;
using ArgusBot.DAL.Repositories.Interfaces;
using ArgusBot.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgusBot.DAL.Repositories.Implementation
{
    public class CheckListRepository : ICheckListRepository
    {
        public readonly MainContext db;

        public CheckListRepository(MainContext mainContext)
        {
            db = mainContext;
        }

        public async Task CreateNewAsync(Check item)
        {
            await db.AddAsync(item);
            await SaveAsync();
        }

        public async Task DeleteAsync(long userId)
        {
            var itemToDelete = db.CheckList.FirstOrDefaultAsync(check => check.UserId.Equals(userId));
            if (itemToDelete is not null)
            {
                db.Entry(itemToDelete).State = EntityState.Deleted;
                await SaveAsync();
            }
        }

        public async Task<ICollection<Check>> GetAllCheckListsAsync()
        {
            return await db.CheckList.ToListAsync();
        }

        public async Task<List<Check>> GetCheckByStatusAsync(StatusTypes status)
        {
            return await db.CheckList.Where(x => x.Status.Equals(status)).ToListAsync();
        }

        public async Task<Check> GetItemByUserIdAsync(long userId)
        {
            return await db.CheckList.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Check item)
        {
            db.Update(item);

            await SaveAsync();
        }
    }
}
