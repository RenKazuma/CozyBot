using Discord_Core.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Discord_Core.Database
{
    public class DatabaseContext : DbContext
    {
        
        public DbSet<User> Users { get; set; }

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
