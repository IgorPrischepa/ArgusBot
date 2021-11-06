using ArgusBot.BLL.DTO;
using System;
using System.Threading.Tasks;

namespace ArgusBot.BLL.Services.Interfaces
{
    public interface IUserService
    {
        public Task<bool> AddTelegramToAccountAsync(Guid userGuid, string telegramId);

        public Task<bool> CreateNewUserAsync(string login, string password);

        public Task<bool> CreateNewUserByTelegramAccountAsync(string telegramId);

        public Task<bool> ChangePasswordAsync(Guid userGuid, string newPassword);

        public Task<Profile> GetUserByLoginAsync(string login);

        public Task<Profile> GetUserByGuidAsync(Guid userGuid);

        public Task<Profile> GetUserByTelegramAccountAsync(string telegramId);
    }
}
