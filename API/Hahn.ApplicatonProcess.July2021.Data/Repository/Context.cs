using Hahn.ApplicationProcess.July2021.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hahn.ApplicationProcess.July2021.Data.Repository
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Asset> Assets { get; set; }

        public Context(DbContextOptions<Context> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseInMemoryDatabase(EnvironmentVariables.DatabaseName);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Entity<Asset>().Property(u => u.Id).ValueGeneratedOnAdd();
        }

        public Context()
        {
        }
    }
}
