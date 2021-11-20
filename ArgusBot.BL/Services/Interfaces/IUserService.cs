using ArgusBot.BL.DTO;
using System;
using System.Threading.Tasks;

namespace ArgusBot.BL.Services.Interfaces
{
    public interface IUserService
    {
        public Task<bool> AddTelegramToAccountAsync(Guid userGuid, string telegramId);

        public Task<bool> CreateNewUserAsync(string login, string password);

        public Task<ProfileDTO> CreateNewUserByTelegramAccountAsync(string telegramId, string userName);

        public Task<bool> ChangePasswordAsync(Guid userGuid, string newPassword);

        public Task<ProfileDTO> GetUserByLoginAsync(string login);

        public Task<ProfileDTO> GetUserByGuidAsync(Guid userGuid);

        public Task<ProfileDTO> GetUserByTelegramAccountAsync(string telegramId);
    }
}
