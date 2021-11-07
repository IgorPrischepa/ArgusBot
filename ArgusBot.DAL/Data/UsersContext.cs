using ArgusBot.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ArgusBot.Data
{
    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UsersContext(DbContextOptions<UsersContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.NormalizedLogin).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.NormalizedTelegramLogin).IsUnique();
        }
    }
}
