using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VdarApi.Helpers;

namespace VdarApi.Models
{
    public class VdarDbContext : DbContext
    {
        public VdarDbContext(DbContextOptions<VdarDbContext> options)
        : base(options)
        {

        }

        public DbSet<Tokens> Tokens { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ConfirmationKey> ConfirmationKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string _salt = SecurePasswordHasherHelper.GenerateSalt();
            string _salt2 = SecurePasswordHasherHelper.GenerateSalt();
            User[] users = new User[]{
                new User {
                    Id = 1,
                    Password = SecurePasswordHasherHelper.Hash("123", _salt),
                    Salt = _salt,
                    Name = "Viktor",
                    SurName = "Bochikalov",
                    FathersName = "Andreevich",
                    PhoneNumber = "7771291221",
                    EmailIsConfirmed = false,
                    PhoneIsConfirmed = true,
                    IsActive = true,
                    Login = "vektor",
                    ActivatedDateUtc = DateTime.UtcNow.AddSeconds(-1212252),
                    Birthday = DateTime.Parse("23.05.1992"),
                    Email = "admin@google.com",
                    CreatedDateUtc = DateTime.UtcNow
                },
                new User {
                    Id = 2,
                    Password = SecurePasswordHasherHelper.Hash("123", _salt2),
                    Salt = _salt2,
                    Name = "Levon",
                    SurName = "Kukuyan",
                    PhoneNumber = "7771940504",
                    PhoneIsConfirmed = false,
                    IsActive = true,
                    CreatedDateUtc = DateTime.UtcNow
                }
            };

            modelBuilder.Entity<User>().HasData(users);

            base.OnModelCreating(modelBuilder);
        }
    }
}
