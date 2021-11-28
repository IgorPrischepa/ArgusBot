using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ArgusBot.BL.Services.Interfaces
{
    public interface IHandleUpdateService
    {
        public Task EchoAsync(Update update);

        public Task HandleErrorAsync(Exception exception);
    }
}
