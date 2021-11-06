using ArgusBot.DAL.Models;
using ArgusBot.DAL.Repositories.Interfaces;
using ArgusBot.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ArgusBot.DAL.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersContext db;

        public UserRepository(UsersContext usersContext)
        {
            db = usersContext;
        }

        public async Task CreateAsync(User user)
        {
            await db.Users.AddAsync(user);
            await SaveAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            User user = await db.Users.FirstAsync(u => u.UserGuid == id);
            db.Entry(user).State = EntityState.Deleted;
            await SaveAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await db.Users.FirstOrDefaultAsync(u => u.UserGuid == id);
        }

        public async Task<User> GetUserByLoginAsync(string login)
        {
            return await db.Users.FirstOrDefaultAsync(u => u.NormalizedLogin == login.ToLower());
        }

        public async Task<User> GetUserByTelegramAccountAsync(string telegramId)
        {
            return await db.Users.FirstOrDefaultAsync(u => u.TelegramId.ToLower() == telegramId.ToLower());
        }

        public async Task UpdateAsync(User user)
        {
            db.Entry(user).State = EntityState.Modified;
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
