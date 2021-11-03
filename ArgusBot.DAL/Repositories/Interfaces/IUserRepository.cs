using ArgusBot.Models;
using System;

namespace ArgusBot.DAL.Repositories.Interfaces
{
    interface IUserRepository
    {
        User GetUser(Guid id);

        void Create(User user);

        void Update(User user);

        void Delete(Guid id);

        void Save();
    }
}
