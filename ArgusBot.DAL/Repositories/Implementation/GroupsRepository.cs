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
    public class GroupsRepository : IGroupsRepository
    {
        private readonly MainContext db;

        public GroupsRepository(MainContext mainContext)
        {
            db = mainContext;
        }

        public async Task AddAdminToGroupAsync(long groupId, long userId)
        {
            Group group = await db.Groups.Include(g => g.GroupAdmins).FirstOrDefaultAsync(g => g.GroupId.Equals(groupId));
            if (group is not null)
            {
                var admin = group.GroupAdmins.SingleOrDefault(ad => ad.TelegramUserId == userId && ad.GroupId == group.Id);
                if (admin != null)
                {
                    return;
                }
                GroupAdmin groupAdmin = new GroupAdmin { TelegramUserId = userId, Group = group };
                await db.GroupAdmins.AddAsync(groupAdmin);
                await SaveChangesAsync();
            }
            else
            {
                throw new ArgumentNullException(nameof(groupId));
            }
        }

        public async Task<Group> AddGroupAsync(Group group)
        {
            if (group is not null)
            {
                var temp = await db.Groups.AddAsync(group);
                await SaveChangesAsync();
                return temp.Entity;
            }
            return null;
        }

        public async Task DeleteAdminFromGroupAsync(long groupId, long userId)
        {
            GroupAdmin groupAdmin = await db.GroupAdmins.FirstOrDefaultAsync(ga => ga.GroupId.Equals(groupId) && ga.TelegramUserId.Equals(userId));
            if (groupAdmin == null)
            {
                db.GroupAdmins.Remove(groupAdmin);
                await SaveChangesAsync();
            }
        }

        public async Task DeleteGroupAsync(long groupId)
        {
            Group group = await GetGroupByIdWithSettingsAsync(groupId);
            db.Groups.Remove(group);
            await SaveChangesAsync();
        }

        public async Task DeleteGroupWithAdmins(long groupId)
        {
            var groups = await db.Groups.Where(g => g.GroupId == groupId).Include(g => g.GroupAdmins).ToListAsync();
            foreach (var group in groups)
            {
                db.GroupAdmins.RemoveRange(group.GroupAdmins);
            }
            db.Groups.RemoveRange(groups);
            await SaveChangesAsync();
        }

        public Task<List<Group>> GetAllGroupAsync()
        {
            return db.Groups.ToListAsync();
        }

        public Task<List<Group>> GetAllGroupWithAdminsAsync()
        {
            return db.Groups.Include(g => g.GroupAdmins).ToListAsync();
        }

        public async Task<Group> GetGroupByIdWithSettingsAsync(long groupId)
        {
            return await db.Groups.Include(x => x.Settings)
                                  .FirstOrDefaultAsync(g => g.GroupId.Equals(groupId));
        }

        public IEnumerable<Group> GetGroupsForUser(long userId)
        {
            return db.Groups.Include(g => g.GroupAdmins)
                            .Include(g => g.Settings)
                            .Where(g => g.GroupAdmins.SingleOrDefault(admin => admin.TelegramUserId == userId) != null)
                            .ToList();
        }

        public async Task SaveChangesAsync()
        {
            await db.SaveChangesAsync();
        }

    }
}
