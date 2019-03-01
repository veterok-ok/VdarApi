﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

            User[] users = new User[]{
                new User {
                    Id = Guid.NewGuid().ToString(),
                    Password = "123",
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
                    Id = Guid.NewGuid().ToString(),
                    Password = "123",
                    Name = "Levon",
                    SurName = "Kukuyan",
                    PhoneNumber = "7771940504",
                    EmailIsConfirmed = true,
                    PhoneIsConfirmed = false,
                    IsActive = true,
                    Login = "lamer",
                    ActivatedDateUtc = DateTime.UtcNow.AddSeconds(-22492),
                    Birthday = DateTime.Parse("13.02.1991"),
                    Email = "l.kukuyan@mail.ru",
                    CreatedDateUtc = DateTime.UtcNow
                }
            };


            modelBuilder.Entity<User>().HasData(users);
            base.OnModelCreating(modelBuilder);
        }
    }
}
