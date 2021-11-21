using Microsoft.Extensions.Configuration;
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

        private readonly string _host;
        private readonly string _botToken;
        private readonly bool _useLongPoll;

        public TelegramHostedService(ITelegramBotClient client, ILogger<TelegramHostedService> logger, IConfiguration configuration)
        {
            _client = client;
            _logger = logger;
            _cancellationTelegramClientTokenSrc = new CancellationTokenSource();

            _host = configuration.GetSection("BotConfiguration").GetValue<string>("HostAddress");
            _botToken = configuration.GetValue<string>("bot-token");

            _useLongPoll = configuration.GetValue<bool>("useLongPollingForTgBot");
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

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (_useLongPoll)
            {
                _logger.LogInformation("Telegram bot will be started with long poll mode.");
                _client.StartReceiving(HandleUpdate, HandleError, null, cancellationToken);
            }
            else
            {
                var webhookAddress = @$"{_host}/bot/{_botToken}";

                _logger.LogInformation("Telegram bot will be started with webhook mode.");
                _logger.LogInformation("Setting webhook.");

                await _client.SetWebhookAsync(
                    url: webhookAddress,
                    allowedUpdates: Array.Empty<UpdateType>(),
                    cancellationToken: cancellationToken);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Initiated a process of stopping telegram host service!");

            if (!_useLongPoll)
            {
                // Remove webhook upon app shutdown
                _logger.LogInformation("Removing webhook");
                await _client.DeleteWebhookAsync(cancellationToken: cancellationToken);
                _logger.LogInformation("Removed webhook");
            }

            _cancellationTelegramClientTokenSrc.Cancel();
        }
    }
}

