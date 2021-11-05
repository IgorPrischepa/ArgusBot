using ArgusBot.DAL.Models;
using ArgusBot.DAL.Repositories.Interfaces;
using ArgusBot.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;


namespace ArgusBot.DAL.Repositories.Implementation
{
    public class UserRepository : IUserRepository
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

        public User GetUserById(Guid id)
        {
            return db.Users.FirstOrDefault(u => u.UserGuid == id);
        }

        public User GetUserByLogin(string login)
        {
            return db.Users.FirstOrDefault(u => u.NormalizedLogin == login.ToLower());
        }

        public User GetUserByTelegramAccount(string telegramId)
        {
            return db.Users.FirstOrDefault(u => u.TelegramId.ToLower() == telegramId.ToLower());
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
