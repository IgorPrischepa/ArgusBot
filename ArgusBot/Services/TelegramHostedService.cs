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

        public TelegramHostedService(ITelegramBotClient client, ILogger<TelegramHostedService> logger)
        {
            _client = client;
            _logger = logger;
        }
        private async Task HandleUpdate(ITelegramBotClient client, Update update, CancellationToken cancelToken)
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
            _client.StartReceiving(new DefaultUpdateHandler(HandleUpdate, HandleError), cancellationToken);
            _logger.LogInformation("Start work of telegram bot");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _client.StopReceiving();
            _logger.LogWarning("Initiated a process of stopping telegram host service!");
            Console.WriteLine(cancellationToken.IsCancellationRequested);
            return Task.CompletedTask;
        }
    }
}
