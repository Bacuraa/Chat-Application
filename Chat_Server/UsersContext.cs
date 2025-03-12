using Microsoft.EntityFrameworkCore;
using Chat_Server.Models;
namespace Chat_Server
{
    public class UsersContext : DbContext
    {
        private const string connectionString = "server=localhost;port=3306;database=chatDB;user=root;password=1";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, MariaDbServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Username);
            modelBuilder.Entity<Contact>().HasKey(x => x.Id);
            modelBuilder.Entity<Message>().HasKey(x => x.Id);

        }

        public DbSet<User> UsersDB { get; set; }
        public DbSet<Contact> ContactsDB { get; set; }
        public DbSet<Message> MessagesDB { get; set; }
    }
}
