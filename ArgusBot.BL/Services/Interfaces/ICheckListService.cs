using ArgusBot.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArgusBot.BL.Services.Interfaces
{
    public interface ICheckListService
    {
        public Task CreateCheckAsync(long telegramGroupId, long telegramUserId, string correctAnswer);

        public Task ChangeCheckStatusForUserAsync(long userId, long groupId, StatusTypes status);

        public Task DeleteCheckForUser(long userId, long groupId);

        public Task UpdateQuestionMsgId(long userId, long groupId, int messageId);

        IAsyncEnumerable<IEnumerable<Check>> GetCheckListWithStatus(StatusTypes status, int count);

        public Task<ICollection<Check>> GetAllFromCheckListAsync();
        public Task<Check> GetCheckForUser(long telegramUserId, long groupId);
    }
}
