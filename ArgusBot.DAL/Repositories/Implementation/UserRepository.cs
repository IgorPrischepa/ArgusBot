using ArgusBot.DAL.Repositories.Interfaces;
using ArgusBot.Data;
using ArgusBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;


namespace ArgusBot.DAL.Repositories.Implementation
{
    class UserRepository : IUserRepository
    {
        private readonly UsersContext db;

        public UserRepository(UsersContext usersContext)
        {
            db = usersContext;
        }

        public void Create(User user)
        {
            db.Users.Add(user);
            Save();
        }

        public void Delete(Guid id)
        {
            User user = db.Users.First(u => u.UserGuid == id);
            Save();
        }

        public User GetUser(Guid id)
        {
            return db.Users.First(u => u.UserGuid == id);
        }

        public User GetUser(string login)
        {
            return db.Users.First(u => u.NormalizedLogin == login.ToLower());
        }

        public User GetUserByTelegramAccount(string telegramId)
        {
            return db.Users.First(u => u.TelegramId == telegramId.ToLower());
        }
        public void Update(User user)
        {
            db.Entry(user).State = EntityState.Modified;
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
