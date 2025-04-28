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
                    Nome = "Carmelita",
                    Cognome = "Kleehuhler",
                    Username = "ckleehuhler0",
                    Ruolo = "User"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "sgowrie1@amazon.com",
                    Password = "$2a$04$ZoH0ul/L.YTCAIz9WmazMe.tHC5s9soNbxq5vyv5QXV845IiN4mNq", 
                    Nome = "Stace",
                    Cognome = "Gowrie",
                    Username = "sgowrie1",
                    Ruolo = "User"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "vdenmead2@mozilla.org",
                    Password = "$2a$04$T1PK56EZ5h/moAeCEFhc0egNZLoB8lrYsTw4G2FPURBaGQoqNzz8m", 
                    Nome = "Valry",
                    Cognome = "Denmead",
                    Username = "vdenmead2",
                    Ruolo = "User"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "dbalwin3@biglobe.ne.jp",
                    Password = "$2a$04$hQ.9pXMdRlu4HUCShaFNiuH95207.YKIJrE6lYXtD/hf5dEwGIwti", 
                    Nome = "Daven",
                    Cognome = "Balwin",
                    Username = "dbalwin3",
                    Ruolo = "Manager"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "fhallede4@themeforest.net",
                    Password = "$2a$04$6b5J1MvETageDvIKaW6jkuYqLdV5nm6soxKNokrCtZzr1nIyehts2", 
                    Nome = "Franzen",
                    Cognome = "Hallede",
                    Username = "fhallede4",
                    Ruolo = "Manager"
                });


            context.SaveChanges();
        }


        //************ INITIALIZING TASKS (PROBABLY TO BE REMOVED IN PRODUCTION) *************
        public static void InitializeTasks(TemplateDbContext context)
        {
            if (context.Tasks.Any())
                return; // Data already present

            context.Tasks.AddRange(

                new Task
                {
                    Id = Guid.NewGuid(),
                    IdCreatore = Guid.NewGuid(), //DA RIGUARDARE (non si può mettere a null)
                    Stato = "Assegnabile",
                    Titolo = "Migrazione_Teams",
                    Descrizione = "Questa è una descrizione"

                },
                new Task
                {
                    Id = Guid.NewGuid(),
                    IdCreatore = Guid.NewGuid(), //DA RIGUARDARE (non si può mettere a null)
                    Stato = "Assegnato",
                    Titolo = "Questo è un task",
                    Descrizione = "Questa è una descrizione"

                }

                );
            context.SaveChanges();
        }
    }
}
