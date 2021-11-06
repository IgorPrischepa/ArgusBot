using System.Threading.Tasks;

namespace ArgusBot.BLL.Services.Interfaces
{
    public interface ISignInService
    {
        public Task<bool> AuthenticateAsync(string login, string password);

        public Task<bool> AuthenticateByTelegramAccountAsync(string telegramId);

        public Task LogoutAsync();
    }
}
