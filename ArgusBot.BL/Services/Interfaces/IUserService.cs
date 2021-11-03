using ArgusBot.BLL.DTO;
using System;

namespace ArgusBot.BLL.Services.Interfaces
{
    interface IUserService
    {
        public bool Authorize(string login, string password);

        public bool AuthorizeByTelegramAccount(string telegramId);

        public bool AddTelegramToAccount(Guid userGuid, string telegramId);

        public bool CreateNewUser(string login, string password);

        public bool CreateNewUserByTelegramAccount(string telegramId);

        public bool ChangePassword(Guid userGuid, string newPassword);

        public Profile GetUser(string login);

        public Profile GetUser(Guid login);

        public Profile GetUserbyTelegramAccount(string telegramId);

        public void Logout();
    }
}
