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

            /* // Rimuove tutti gli utenti esistenti (uncomment only for testing)
             context.Users.RemoveRange(context.Users);
             context.SaveChanges();*/

            context.Users.AddRange(
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "ckleehuhler0@furl.net",
                    Password = "M0Cuk9OsrcS/rTLGf5SY6DUPqU2rGc1wwV2IL88GVGo=", // SHA-256 of text "Prova"
                    Nome = "Carmelita",
                    Cognome = "Kleehuhler",
                    Username = "ckleehuhler0",
                    Ruolo = "Developer"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "sgowrie1@amazon.com",
                    Password = "M0Cuk9OsrcS/rTLGf5SY6DUPqU2rGc1wwV2IL88GVGo=", // SHA-256 of text "Prova"
                    Nome = "Stace",
                    Cognome = "Gowrie",
                    Username = "sgowrie1",
                    Ruolo = "Developer"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "vdenmead2@mozilla.org",
                    Password = "M0Cuk9OsrcS/rTLGf5SY6DUPqU2rGc1wwV2IL88GVGo=", // SHA-256 of text "Prova"
                    Nome = "Valry",
                    Cognome = "Denmead",
                    Username = "vdenmead2",
                    Ruolo = "Developer"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "dbalwin3@biglobe.ne.jp",
                    Password = "M0Cuk9OsrcS/rTLGf5SY6DUPqU2rGc1wwV2IL88GVGo=", // SHA-256 of text "Prova"
                    Nome = "Daven",
                    Cognome = "Balwin",
                    Username = "dbalwin3",
                    Ruolo = "ResponsabileInterno"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "fhallede4@themeforest.net",
                    Password = "M0Cuk9OsrcS/rTLGf5SY6DUPqU2rGc1wwV2IL88GVGo=", // SHA-256 of text "Prova"
                    Nome = "Franzen",
                    Cognome = "Hallede",
                    Username = "fhallede4",
                    Ruolo = "ResponsabileEsterno"
                });


            context.SaveChanges();
        }


        public static void InitializeTasks(TemplateDbContext context)
        {
            /*if (context.Tasks.Any())
                return;*/

             // Rimuove tutti gli utenti esistenti (uncomment only for testing)
             context.Tasks.RemoveRange(context.Tasks);
             context.SaveChanges();

            // Recupera un utente qualsiasi come creatore (dinamico)
            var creatore = context.Users.FirstOrDefault();
            if (creatore == null)
                throw new Exception("Nessun utente trovato per assegnare i task.");

            context.Tasks.AddRange(
                new Task
                {
                    Id = Guid.NewGuid(),
                    IdCreatore = creatore.Id,
                    Stato = "InAttesa",
                    Titolo = "Migrazione Teams",
                    Descrizione = "Effettuare la migrazione dei canali Teams del reparto marketing.",
                    Tipologia = "Interno",
                    Priorità = 0,
                    DataCreazione = DateTime.Now,
                    DataScadenza = DateTime.Today.AddDays(5)
                },
                new Task
                {
                    Id = Guid.NewGuid(),
                    IdCreatore = creatore.Id,
                    Stato = "InAttesa",
                    Titolo = "Sviluppo Landing Page",
                    Descrizione = "Creare una landing page responsive per la campagna primavera.",
                    Tipologia = "Esterno",
                    Priorità = 1,
                    DataCreazione = DateTime.Now,
                    DataScadenza = DateTime.Today.AddDays(10)
                },
                new Task
                {
                    Id = Guid.NewGuid(),
                    IdCreatore = creatore.Id,
                    Stato = "InAttesa",
                    Titolo = "Fix bug autenticazione",
                    Descrizione = "Correggere errore sul login con credenziali errate.",
                    Tipologia = "Interno",
                    Priorità = 2,
                    DataCreazione = DateTime.Now,
                    DataScadenza = DateTime.Today.AddDays(3)
                }
            );

            context.SaveChanges();
        }

    }
}
