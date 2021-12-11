using ArgusBot.DAL.Models;
using ArgusBot.DAL.Repositories.Interfaces;
using ArgusBot.Data;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task DeleteAsync(long userId, long groupId)
        {
            var itemToDelete = db.CheckList.FirstOrDefault(check => check.UserId.Equals(userId) && check.GroupId == groupId);
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

        public async Task<IEnumerable<Check>> GetCheckByStatusAndLimitAsync(StatusTypes status, int countChunk, int offset)
        {
            DateTime maxSendingDateTime = DateTime.Now.Subtract(TimeSpan.FromSeconds(30));
            IEnumerable<Check> chunk = await db.CheckList.Where(u => u.Status == status && u.SendingTime <= maxSendingDateTime)
                                                          .Skip(offset).Take(countChunk).ToListAsync();
            return chunk;
        }
        public async Task<Check> GetItemByUserAndGroupIdAsync(long userId, long groupId)
        {
            return await db.CheckList.SingleOrDefaultAsync(u => u.UserId == userId && u.GroupId == groupId);
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
