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


        //************ INITIALIZING TASKS (PROBABLY TO BE REMOVED IN PRODUCTION) *************
        public static void InitializeTasks(TemplateDbContext context)
        {
           if (context.Tasks.Any())
                return; // Data already present
            
             // Rimuove tutti i task esistenti (uncomment only for testing)
             /*context.Tasks.RemoveRange(context.Tasks);
             context.SaveChanges();*/

            context.Tasks.AddRange(

                new Task
                {
                    Id = Guid.NewGuid(),
                    IdCreatore = Guid.Parse("b3a42cfa-5f32-47e3-b6a4-1d9c4e6a1234"),
                    IdAssegnatario = null,
                    Priorità = "Bassa",
                    Stato = "InAttesa",
                    Titolo = "Aggiornare il sistema operativo dei server",
                    Tipologia = "Interno",
                    Descrizione = "Programmare l'aggiornamento per evitare downtime durante l'orario lavorativo.",
                    DataCreazione = DateTime.UtcNow,
                    DataScadenza = DateTime.UtcNow.AddDays(7)
                },
                new Task
                {
                    Id = Guid.NewGuid(),
                    IdCreatore = Guid.Parse("a1f9e8b7-91f0-4f3a-9c3a-99f4e2a1b567"),
                    IdAssegnatario = null,
                    Priorità = "Media",
                    Stato = "InAttesa",
                    Titolo = "Revisione policy di sicurezza",
                    Tipologia = "Interno",
                    Descrizione = "Analizzare e aggiornare le policy interne sulla sicurezza informatica.",
                    DataCreazione = DateTime.UtcNow,
                    DataScadenza = DateTime.UtcNow.AddDays(10)
                },
                new Task
                {
                    Id = Guid.NewGuid(),
                    IdCreatore = Guid.Parse("bb2e5d4a-33a3-4db2-8576-1ab4f5d09123"),
                    IdAssegnatario = null,
                    Priorità = "Alta",
                    Stato = "InAttesa",
                    Titolo = "Contattare fornitore per aggiornamento contratto",
                    Tipologia = "Esterno",
                    Descrizione = "Raccogliere preventivi e ridefinire i termini del contratto con il fornitore principale.",
                    DataCreazione = DateTime.UtcNow,
                    DataScadenza = DateTime.UtcNow.AddDays(5)
                },
                new Task
                {
                    Id = Guid.NewGuid(),
                    IdCreatore = Guid.Parse("ccdfcce7-934f-4ea1-9ba4-8b7c7b6c5678"),
                    IdAssegnatario = Guid.Parse("d3e55c00-3456-4a93-b643-9ddc4fa0efab"),
                    Priorità = "Media",
                    Stato = "InLavorazione",
                    Titolo = "Preparare presentazione per il cliente",
                    Tipologia = "Esterno",
                    Descrizione = "Creare slide PowerPoint per illustrare le funzionalità del nuovo prodotto.",
                    DataCreazione = DateTime.UtcNow.AddDays(-2),
                    DataScadenza = DateTime.UtcNow.AddDays(3)
                },
                new Task
                {
                    Id = Guid.NewGuid(),
                    IdCreatore = Guid.Parse("f7bb8c1e-8a17-4a0b-b1b5-9d2b0b1e2345"),
                    IdAssegnatario = Guid.Parse("a8c1a8e3-1234-4567-b3bb-98765e4f1234"),
                    Priorità = "Bassa",
                    Stato = "Completato",
                    Titolo = "Verifica backup mensile",
                    Tipologia = "Interno",
                    Descrizione = "Controllare che tutti i backup dei dati siano stati completati correttamente.",
                    DataCreazione = DateTime.UtcNow.AddDays(-10),
                    DataScadenza = DateTime.UtcNow.AddDays(-2)
                }

            );
            context.SaveChanges();
        }
    }
}
