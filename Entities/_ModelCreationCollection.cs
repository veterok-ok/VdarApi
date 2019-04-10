using Entities.Models;
using Helpers.Security;
using System;

namespace Entities
{
    public static class _ModelCreationCollection
    {
        public static User[] GetUsers()
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
                    CityId = 1,
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
                    CityId = 1,
                    PhoneIsConfirmed = false,
                    IsActive = true,
                    CreatedDateUtc = DateTime.UtcNow
                }
            };
            return users;
        }
               
        public static Country[] GetCountries()
        {
            return new Country[]
            {
               new Country
               {
                   Id = 1,
                   Name = "Казахстан"
               }
            };
        }

        public static City[] GetCities()
        {
            return new City[]
            {
               new City
               {
                   Id = 1,
                   CountryId = 1,
                   Name = "Алматы"
               },
               new City
               {
                   Id = 2,
                   CountryId = 1,
                   Name = "Нур-Султан"
               },
               new City
               {
                   Id = 3,
                   CountryId = 1,
                   Name = "Караганда"
               },
               new City
               {
                   Id = 4,
                   CountryId = 1,
                   Name = "Кызылорда"
               },
               new City
               {
                   Id = 5,
                   CountryId = 1,
                   Name = "Тараз"
               },
               new City
               {
                   Id = 6,
                   CountryId = 1,
                   Name = "Семипалатинск"
               },
               new City
               {
                   Id = 7,
                   CountryId = 1,
                   Name = "Павлодар"
               }
            };
        }
    }
}
