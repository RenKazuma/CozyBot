using Discord_Core.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Timer = Discord_Core.Database.Entities.Timer;

namespace Discord_Core.Database
{
    public class DatabaseContext : DbContext
    {
        
        public DbSet<User> users { get; set; }

        public DbSet<Buff> buffs { get; set; }

        public DbSet<Timer> timers { get; set; }

        public DbSet<WaitingList> waitingLists { get; set; }

        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Service.GetConnectionString());
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    }
}
