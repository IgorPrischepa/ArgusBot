using ArgusBot.BL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ArgusBot.BL.Services.Implementation
{
    public class HandleUpdateService : IHandleUpdateService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<HandleUpdateService> _logger;
        private readonly ICaptchaService _captchaService;
        private readonly IGroupService _groupService;
        private readonly ICheckListService _checkListService;
        private readonly IConfiguration _config;

        public HandleUpdateService(ITelegramBotClient botClient, ILogger<HandleUpdateService> logger, ICaptchaService captchaService, IGroupService groupService, ICheckListService checkListService, IConfiguration config)
        {
            _botClient = botClient;
            _logger = logger;
            _captchaService = captchaService;
            _groupService = groupService;
            _checkListService = checkListService;
            _config = config;
        }

        public async Task EchoAsync(Update update)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(update.Message!),
                UpdateType.MyChatMember => BotOnChatEventReceived(update.MyChatMember),
                _ => UnknownUpdateHandlerAsync(update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(exception);
            }
        }

        private async Task BotOnChatEventReceived(ChatMemberUpdated myChatMember)
        {
            myChatMember.VerifyNotNull();
            if (myChatMember.NewChatMember.User != null)
            {
                if (myChatMember.NewChatMember.User.Id == long.Parse(_config["bot-id"]) && !await _groupService.Exists(myChatMember.Chat.Id))
                {
                    _logger.LogInformation($"Bot was added in group {myChatMember.Chat.Id}");
                    await _groupService.AddGroupAsync(myChatMember.Chat.Id, myChatMember.Chat.Title);
                    await CheckAdmins(myChatMember);
                    return;
                }
                if (myChatMember.NewChatMember.Status == ChatMemberStatus.Left && myChatMember.NewChatMember.User.Id == long.Parse(_config["bot-id"]))
                {
                    await OnBotWasRemoved(myChatMember);
                    return;
                }
                if (myChatMember.NewChatMember.Status == ChatMemberStatus.Kicked || myChatMember.NewChatMember.Status == ChatMemberStatus.Left)
                {
                    await OnUserWasRemoved(myChatMember);
                    return;
                }
                if (myChatMember.NewChatMember.Status == ChatMemberStatus.Administrator)
                {
                    await CheckAdmins(myChatMember);
                }
            }
        }

        private async Task BotOnMessageReceived(Message message)
        {
            if (message.From.Id == long.Parse(_config["bot-id"]))
            {
                return;
            }
            if (message.NewChatMembers != null && message.Type == MessageType.ChatMembersAdded)
            {
                var members = RemoveBotFromNewChatMembers(message.NewChatMembers);
                if (members.Count() > 0)
                {
                    _logger.LogInformation($"Attempt to join chat {message.Chat.Id}. Initializing a captcha for new users");
                    await _captchaService.InitiateCaptchaProcess(message.NewChatMembers, message.Chat.Id);
                }
            }
            if (message.Type == MessageType.ChatMemberLeft)
            {
                await _checkListService.DeleteCheckForUser(message.LeftChatMember.Id);
                _logger.LogInformation($"Captcha data for this user was removed");
            }
            await _captchaService.ProcessMesagge(message);
            _logger.LogInformation("Receive message type: {messageType}", message.Type);
            if (message.Type != MessageType.Text)
                return;

            await _botClient.SendTextMessageAsync(message.Chat.Id, message.Text);
        }

        public Task ExceptionFromUpdateHandlerAsync(Update update)
        {
            _logger.LogInformation("Unknown update type: {updateType}", update.Type);
            return Task.CompletedTask;
        }

        private Task UnknownUpdateHandlerAsync(Update update)
        {
            _logger.LogInformation("Unknown update type: {updateType}", update.Type);
            return Task.CompletedTask;
        }

        public Task HandleErrorAsync(Exception exception)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);
            return Task.CompletedTask;
        }
        private async Task CheckAdmins(ChatMemberUpdated myChatMember)
        {
            _logger.LogInformation($"bot is checking admins from {myChatMember.Chat.Id} group");
            var adminsFromTlg = await _botClient.GetChatAdministratorsAsync(myChatMember.Chat.Id);
            foreach (var admin in adminsFromTlg)
            {
                await _groupService.AddAdminToTelegramGroupAsync(myChatMember.Chat.Id, admin.User.Id);
            }
        }
        private IEnumerable<User> RemoveBotFromNewChatMembers(IEnumerable<User> members)
        {
            var bot = members.SingleOrDefault(u => u.Id == long.Parse(_config["bot-id"]));
            if (bot != null)
            {
                members.ToList().Remove(bot);
            }
            return members;
        }
        private async Task OnBotWasRemoved(ChatMemberUpdated chatMemberUpdated)
        {
            _logger.LogInformation($"User {chatMemberUpdated.NewChatMember.User.Id} was removed from the group");
            await _groupService.RemoveGroupWithAdmins(chatMemberUpdated.Chat.Id);
        }
        private async Task OnUserWasRemoved(ChatMemberUpdated chatMemberUpdated)
        {
            switch (chatMemberUpdated.NewChatMember.Status)
            {
                case ChatMemberStatus.Kicked:
                    {
                        _logger.LogInformation($"User {chatMemberUpdated.NewChatMember.User.Id} was removed from the group");
                        break;
                    }
                case ChatMemberStatus.Left:
                    {
                        _logger.LogInformation($"User {chatMemberUpdated.NewChatMember.User.Id} has left the chat. Removing captcha result for him.");
                        break;
                    }
                default: return;
            }
            await _checkListService.DeleteCheckForUser(chatMemberUpdated.NewChatMember.User.Id);
        }
    }
}
