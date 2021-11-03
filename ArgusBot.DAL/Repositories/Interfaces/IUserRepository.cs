using ArgusBot.DAL.Models;
using System;

namespace ArgusBot.DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User GetUserById(Guid id);

        User GetUserByLogin(string login);

        User GetUserByTelegramAccount(string telegramId);

        void Create(User user);

        void Update(User user);

        void Delete(Guid id);

        void Save();
    }
}
