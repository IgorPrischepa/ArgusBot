namespace ArgusBot.BLL.Services.Interfaces
{
    public interface ISignInService
    {
        public bool Authorize(string login, string password);

        public bool AuthorizeByTelegramAccount(string telegramId);

        public void Logout();
    }
}
