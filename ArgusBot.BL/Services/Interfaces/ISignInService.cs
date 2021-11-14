using System.Threading.Tasks;

namespace ArgusBot.BL.Services.Interfaces
{
    public interface ISignInService
    {
        public Task<bool> AuthenticateAsync(string login, string password);

        public Task<bool> AuthenticateByTelegramAccountAsync(string telegramId);

        public Task LogoutAsync();
    }
}
