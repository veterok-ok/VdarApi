using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class VdarDbContext : DbContext
    {
        public VdarDbContext(DbContextOptions<VdarDbContext> options)
        : base(options)
        {

        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        
        public DbSet<User> Users { get; set; }
        public DbSet<ConfirmationKey> ConfirmationKeys { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasData(_ModelCreationCollection.GetCountries());
            modelBuilder.Entity<City>().HasData(_ModelCreationCollection.GetCities());

            modelBuilder.Entity<User>().HasData(_ModelCreationCollection.GetUsers());

            base.OnModelCreating(modelBuilder);
        }
    }
}
