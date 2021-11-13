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
        private CancellationTokenSource _cancellationTelegramClientTokenSrc;
        public TelegramHostedService(ITelegramBotClient client, ILogger<TelegramHostedService> logger)
        {
            _client = client;
            _logger = logger;
            _cancellationTelegramClientTokenSrc = new CancellationTokenSource();
        }
        private async Task HandleUpdate(ITelegramBotClient client, Update update, CancellationToken cancelToken)
        {
            cancelToken.ThrowIfCancellationRequested();
            if (update.Type == UpdateType.Message)
            {
                _logger.LogInformation("Message was received!");
                await _client.SendTextMessageAsync(update.Message.Chat.Id, update.Message.Text);
            }
        }
        private Task HandleError(ITelegramBotClient client, Exception exception, CancellationToken cancelToken)
        {
            cancelToken.ThrowIfCancellationRequested();
            _logger.LogError(exception.Message);
            return Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Start work of telegram bot");
                _client.StartReceiving(new DefaultUpdateHandler(HandleUpdate, HandleError), _cancellationTelegramClientTokenSrc.Token);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Telegram bot already has not received updates");
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Initiated a process of stopping telegram host service!");
            _cancellationTelegramClientTokenSrc.Cancel();
            return Task.CompletedTask;
        }
    }
}
