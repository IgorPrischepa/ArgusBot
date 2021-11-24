using ArgusBot.BL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        //private readonly IServiceProvider _serviceProvider;

        private readonly string _host;
        private readonly string _botToken;
        private readonly bool _useLongPoll;

        private readonly IHandleUpdateService _handleUpdateService;

        public TelegramHostedService(ITelegramBotClient client, ILogger<TelegramHostedService> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _client = client;
            _logger = logger;
            _cancellationTelegramClientTokenSrc = new CancellationTokenSource();
            _host = configuration.GetValue<string>("bot-host");
            _botToken = configuration.GetValue<string>("bot-token");
            _useLongPoll = configuration.GetValue<bool>("useLongPollingForTgBot");


            IServiceScope scope = serviceProvider.CreateScope();
            IHandleUpdateService handleUpdateService =
                scope.ServiceProvider.GetRequiredService<IHandleUpdateService>();

            _handleUpdateService = handleUpdateService;

        }

        private async Task HandleUpdate(ITelegramBotClient client, Update update, CancellationToken cancelToken)
        {
            cancelToken.ThrowIfCancellationRequested();

            _logger.LogInformation("Message was received!");

            await _handleUpdateService.EchoAsync(update);
        }

        private async Task HandleError(ITelegramBotClient client, Exception exception, CancellationToken cancelToken)
        {
            cancelToken.ThrowIfCancellationRequested();

            await _handleUpdateService.HandleErrorAsync(exception);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {

            if (_useLongPoll)
            {
                _logger.LogInformation("Telegram bot will be started with long poll mode.");

                _logger.LogInformation("Checking for the disabling of a web hook.");

                var info = await _client.GetWebhookInfoAsync(cancellationToken);

                if (info.Url != string.Empty)
                {
                    _logger.LogInformation("The webhook has not been removed.");

                    await RemoveWebhook(cancellationToken);
                }

                _client.StartReceiving(new DefaultUpdateHandler(HandleUpdate, HandleError), _cancellationTelegramClientTokenSrc.Token);
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
                await RemoveWebhook(cancellationToken);
            }

            _cancellationTelegramClientTokenSrc.Cancel();
        }

        private async Task RemoveWebhook(CancellationToken cancellationToken)
        {
            // Remove webhook upon app shutdown
            _logger.LogInformation("Removing webhook");
            await _client.DeleteWebhookAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("Removed webhook");
        }
    }
}

