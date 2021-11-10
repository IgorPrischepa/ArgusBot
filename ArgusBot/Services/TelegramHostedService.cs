using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ArgusBot.Services.Implementations
{
    public class TelegramHostedService : IHostedService
    {
        private readonly ITelegramBotClient _client;
        private readonly ILogger<TelegramHostedService> _logger;
        private readonly IHostApplicationLifetime _lifetime;

        public TelegramHostedService(ITelegramBotClient client, ILogger<TelegramHostedService> logger, IHostApplicationLifetime lifetime)
        {
            _client = client;
            _logger = logger;
            _lifetime = lifetime;
        }
        private async Task HandleUpdate(ITelegramBotClient client,Update update,CancellationToken cancelToken)
        {
            if (update.Type == UpdateType.Message)
            {
                _logger.LogInformation("Message was received!");
                await _client.SendTextMessageAsync(update.Message.Chat.Id, update.Message.Text);
            }   
        }
        private Task HandleError(ITelegramBotClient client, Exception exception, CancellationToken cancelToken)
        {
            _logger.LogError(exception.Message);
            return Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _client.StartReceiving(new DefaultUpdateHandler(HandleUpdate, HandleError), _lifetime.ApplicationStopping);
            _logger.LogInformation("Start work of telegram bot");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning("It`s beginning a process of stopping telegram host service!");
            }
            return Task.CompletedTask;
        }
    }
}
