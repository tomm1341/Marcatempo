using Template.Services.Shared;
using System;
using System.Linq;
using Template.Services;

namespace Template.Infrastructure
{
    public class DataGenerator
    {
        public static void InitializeUsers(TemplateDbContext context)
        {
            if (context.Users.Any())
            {
                return;   // Data was already seeded
            }

            context.Users.AddRange(
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "ckleehuhler0@furl.net",
                    Password = "M0Cuk9OsrcS/rTLGf5SY6DUPqU2rGc1wwV2IL88GVGo=", // SHA-256 of text "Prova"
                    FirstName = "Carmelita",
                    LastName = "Kleehuhler",
                    NickName = "ckleehuhler0",
                    Role = "User"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "sgowrie1@amazon.com",
                    Password = "$2a$04$ZoH0ul/L.YTCAIz9WmazMe.tHC5s9soNbxq5vyv5QXV845IiN4mNq", 
                    FirstName = "Stace",
                    LastName = "Gowrie",
                    NickName = "sgowrie1",
                    Role = "User"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "vdenmead2@mozilla.org",
                    Password = "$2a$04$T1PK56EZ5h/moAeCEFhc0egNZLoB8lrYsTw4G2FPURBaGQoqNzz8m", 
                    FirstName = "Valry",
                    LastName = "Denmead",
                    NickName = "vdenmead2",
                    Role = "User"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "dbalwin3@biglobe.ne.jp",
                    Password = "$2a$04$hQ.9pXMdRlu4HUCShaFNiuH95207.YKIJrE6lYXtD/hf5dEwGIwti", 
                    FirstName = "Daven",
                    LastName = "Balwin",
                    NickName = "dbalwin3",
                    Role = "Manager"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "fhallede4@themeforest.net",
                    Password = "$2a$04$6b5J1MvETageDvIKaW6jkuYqLdV5nm6soxKNokrCtZzr1nIyehts2", 
                    FirstName = "Franzen",
                    LastName = "Hallede",
                    NickName = "fhallede4",
                    Role = "Manager"
                });


            context.SaveChanges();
        }
    }
}
