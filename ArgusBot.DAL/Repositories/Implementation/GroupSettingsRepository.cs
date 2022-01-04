using System.Linq;
using System.Threading.Tasks;

using ArgusBot.DAL.Models;
using ArgusBot.DAL.Repositories.Interfaces;
using ArgusBot.Data;

using Microsoft.EntityFrameworkCore;

namespace ArgusBot.DAL.Repositories.Implementation
{
    public class GroupSettingsRepository : IGroupSettingsRepository
    {
        private readonly MainContext _db;

        public GroupSettingsRepository(MainContext mainContext)
        {
            _db = mainContext;
        }

        public async Task AddSettingsAsync(GroupSettings groupSettings)
        {
            await _db.GroupsSettings.AddAsync(groupSettings);
            await SaveChangesAsync();
        }

        public async Task<GroupSettings> GetSettingAsync(long groupId)
        {
            return await _db.GroupsSettings.FirstOrDefaultAsync(x => x.TelegramChatId.Equals(groupId));
        }

        public async Task<bool> UpdateSettingsGroupAsync(GroupSettings groupSettings)
        {
            if (groupSettings != null)
            {
                _db.GroupsSettings.Update(groupSettings);
                await SaveChangesAsync();
                return true;
            }

            return false;
        }

        private async Task SaveChangesAsync() => await _db.SaveChangesAsync();
    }
}
