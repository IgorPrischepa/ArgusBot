using ArgusBot.BL.Services.Interfaces;
using ArgusBot.DAL.Models;
using ArgusBot.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArgusBot.BL.Services.Implementation
{
    public class CheckService : ICheckListServiceInterface
    {
        private readonly ICheckListRepository checkList;

        public CheckService(ICheckListRepository checkListRepository)
        {
            checkList = checkListRepository;
        }

        public async Task ChangeCheckStatusForUserAsync(int userId, StatusTypes status)
        {
            var userToUpdate = await checkList.GetItemByUserIdAsync(userId);
            if (userToUpdate != null)
            {
                userToUpdate.Status = status;
                await checkList.UpdateAsync(userToUpdate);
            }
        }

        public async Task CreateCheckAsync(Check checkItem)
        {
            if (checkItem is null) throw new ArgumentNullException(nameof(checkItem));
            if (checkItem.CorrectAnswer is null || checkItem.CorrectAnswer == string.Empty) throw new ArgumentException("Answer can't be empty or null");
            if (checkItem.UserId == 0 || checkItem.UserId < 0) throw new ArgumentException("User id can't be zero or negative");
            if (checkItem.GroupId == 0 || checkItem.GroupId < 0) throw new ArgumentException("Group id can't be zero or negative");

            await checkList.CreateNewAsync(checkItem);
        }

        public async Task DeleteCheckForUser(int userId)
        {
            if (userId == 0 || userId < 0) throw new ArgumentException("User id can't be zero or negative");
            await checkList.DeleteAsync(userId);
        }

        public async Task UpdateQuestionMsgId(int userId, int messageId)
        {
            if (userId == 0 || userId < 0) throw new ArgumentException("User id can't be zero or negative");
            if (messageId == 0 || messageId < 0) throw new ArgumentException("Message id can't be zero or negative");

            var itemToUpdate = await checkList.GetItemByUserIdAsync(userId);

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
    }
}
