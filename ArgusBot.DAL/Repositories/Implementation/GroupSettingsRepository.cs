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
        private readonly MainContext db;

        public GroupSettingsRepository(MainContext mainContext)
        {
            db = mainContext;
        }

        public async Task AddSettingsAsync(GroupSettings groupSettings)
        {
            await db.GroupsSettings.AddAsync(groupSettings);
            await SaveChangesAsync();
        }

        public async Task CreateSettingsForNewGroupAsync(int groupId)
        {
            GroupSettings gs = new GroupSettings
            {

            };
            await db.GroupsSettings.AddAsync(gs);
            await SaveChangesAsync();
        }

        public async Task<GroupSettings> GetSettingAsync(long groupId)
        {
            return await db.GroupsSettings.FirstOrDefaultAsync(x => x.TelegramChatId.Equals(groupId));
        }

        public async Task<bool> UpdateSettingsGroupAsync(GroupSettings groupSettings)
        {
            if (groupSettings != null)
            {
                db.GroupsSettings.Update(groupSettings);
                await SaveChangesAsync();
                return true;
            }

            return false;
        }

        private async Task SaveChangesAsync() => await db.SaveChangesAsync();
    }
}
