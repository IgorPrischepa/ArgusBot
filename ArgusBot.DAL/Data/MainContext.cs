using ArgusBot.DAL.Models;

using Microsoft.EntityFrameworkCore;

namespace ArgusBot.Data
{
    public class MainContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Check> CheckList { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<GroupAdmin> GroupAdmins { get; set; }

        public DbSet<GroupSettings> GroupsSettings { get; set; }

        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.NormalizedLogin).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.NormalizedTelegramLogin).IsUnique();
            modelBuilder.Entity<Check>().HasIndex("GroupId", "UserId").IsUnique();
        }
    }
}
