using ArgusBot.DAL.Models;
using System;
using System.Threading.Tasks;

namespace ArgusBot.DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(Guid id);

        Task<User> GetUserByLoginAsync(string login);

        Task<User> GetUserByTelegramAccountAsync(string telegramId);

        Task CreateAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(Guid id);

        Task SaveAsync();
    }
}
