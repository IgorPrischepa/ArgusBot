using ArgusBot.DAL.Models;
using System;

namespace ArgusBot.DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public User GetUserById(Guid id);

        public User GetUserByLogin(string login);

        public User GetUserByTelegramAccount(string telegramId);

        public void Create(User user);

        public void Update(User user);

        public void Delete(Guid id);

        void Save();
    }
}
