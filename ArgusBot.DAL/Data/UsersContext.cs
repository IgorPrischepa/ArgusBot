using Microsoft.EntityFrameworkCore;
using ArgusBot.Models;

namespace ArgusBot.Data
{
    public class UsersContext : DbContext
    {
        DbSet<User> Users { get; set; }

        public UsersContext(DbContextOptions<UsersContext> options) : base(options)
        {

        }
    }
}
