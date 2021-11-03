using ArgusBot.BLL.DTO;
using System;

namespace ArgusBot.BLL.Services.Interfaces
{
    public interface IUserService
    {
        public bool AddTelegramToAccount(Guid userGuid, string telegramId);

        public bool CreateNewUser(string login, string password);

        public bool CreateNewUserByTelegramAccount(string telegramId);

        public bool ChangePassword(Guid userGuid, string newPassword);

        public Profile GetUserByLogin(string login);

        public Profile GetUserByGuid(Guid userGuid);

        public Profile GetUserByTelegramAccount(string telegramId);
    }
}
