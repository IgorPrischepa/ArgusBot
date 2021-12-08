﻿using ArgusBot.BL.Services.Interfaces;
using ArgusBot.DAL.Models;
using ArgusBot.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArgusBot.BL.Services.Implementation
{
    public class CheckService : ICheckListService
    {
        private readonly ICheckListRepository checkList;

        public CheckService(ICheckListRepository checkListRepository)
        {
            checkList = checkListRepository;
        }

        public async Task ChangeCheckStatusForUserAsync(long userId, byte status)
        {
            var userToUpdate = await checkList.GetItemByUserIdAsync(userId);
            if (userToUpdate != null)
            {
                userToUpdate.Status = (StatusTypes)status;
                await checkList.UpdateAsync(userToUpdate);
            }
        }

        public async Task CreateCheckAsync(long telegramGroupId, long telegramUserId, string correctAnswer)
        {
            if (correctAnswer is null || correctAnswer == string.Empty) throw new ArgumentException("Answer can't be empty or null");
            if (telegramUserId == 0 || telegramUserId < 0) throw new ArgumentException("User id can't be zero or negative");
            var dateSentMessage = DateTime.Now;
            Check check = new Check
            {
                CorrectAnswer = correctAnswer,
                GroupId = telegramGroupId,
                UserId = telegramUserId,
                Status = StatusTypes.InProgress,
                SendingTime = dateSentMessage
            };

            await checkList.CreateNewAsync(check);
        }

        public async Task DeleteCheckForUser(long userId)
        {
            if (userId == 0 || userId < 0) throw new ArgumentException("User id can't be zero or negative");
            await checkList.DeleteAsync(userId);
        }

        public async Task UpdateQuestionMsgId(long userId, int messageId)
        {
            if (userId == 0 || userId < 0) throw new ArgumentException("User id can't be zero or negative");
            if (messageId == 0 || messageId < 0) throw new ArgumentException("Message id can't be zero or negative");

            Check itemToUpdate = await checkList.GetItemByUserIdAsync(userId);

            if (itemToUpdate is not null)
            {
                itemToUpdate.QuestionMessageId = messageId;
                await checkList.UpdateAsync(itemToUpdate);
            }
        }

        public async Task<ICollection<Check>> GetAllFromCheckListAsync()
        {
            return await checkList.GetAllCheckListsAsync();
        }

        public async Task<List<Check>> GetCheckListWithStatus(byte status)
        {
            return await checkList.GetCheckByStatusAsync((StatusTypes)status);
        }

        public async Task<Check> GetCheckForUser(long telegramUserId)
        {
            return await checkList.GetItemByUserIdAsync(telegramUserId);
        }
    }
}
