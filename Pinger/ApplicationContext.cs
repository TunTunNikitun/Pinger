using Microsoft.EntityFrameworkCore;
using Pinger.Models;
namespace Pinger
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Services> Services { get; set; }
        public DbSet<Log> Log { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) :base(options)
        {
            Database.EnsureCreated();
        }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Pinger;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Services>().HasIndex(s => s.Name).IsUnique();
        }
    }
}
