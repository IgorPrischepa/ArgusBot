using Microsoft.EntityFrameworkCore;
using ArgusBot.DAL.Models;

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
            //Property Configurations
            modelBuilder.Entity<User>().HasIndex(u => u.NormalizedLogin).IsUnique();
        }
    }
}
