using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Services.Shared
{
    public class CreateTaskCommand // DTO (solo i campi che vengono passati dal frontend)
    {
        public Guid Id { get; set; } 
        public string Priorità { get; set; }
        public string Stato { get; set; }
        public string Titolo { get; set; }
        public string Tipologia { get; set; }
        public string Descrizione { get; set; }
        public DateTime? DataScadenza { get; set; }
    }

    public class ChangeTaskStatusCommand
    {
        public Guid Id { get; set; }
        public string Stato { get; set; }
    }

    public class DeleteTaskCommand
    {
        public Guid IdTask { get; set; }
        public Guid IdUtente { get; set; }
    }

    public class PresaInCaricoTask
    {
        public Guid TaskId { get; set; }
        public Guid IdAssegnatario { get; set; } // L'utente che prende in carico il task

    }

    public class MarkTaskAsCompleted
    {
        public Guid TaskId { get; set; }
        public Guid IdUtente { get; set; } // Id dell'utente loggato in quel momento che sta segnando come completato il task
    }

    public partial class SharedService
    {
        public async Task<Guid> Handle(CreateTaskCommand cmd, Guid IdCreatore)
        {
            var dataCreazione = DateTime.UtcNow;

            // Validazione: la data di scadenza non può essere prima della data di creazione
            if (cmd.DataScadenza < dataCreazione)
            {
                throw new ArgumentException("La data di scadenza non può essere precedente alla data di creazione.");
            }
            else
            {
                var task = new Task
                {
                    Id = Guid.NewGuid(),
                    IdCreatore = IdCreatore, // Va passato LATO FRONTEND, da gestire nel controller utilizzando i Claim per recuperare l'id
                    Priorità = cmd.Priorità,
                    Stato = cmd.Stato,
                    Titolo = cmd.Titolo,
                    Tipologia = cmd.Tipologia, // Da passare lato frontend, se il responsabile che lo crea è interno o esterno
                    Descrizione = cmd.Descrizione,
                    DataCreazione = dataCreazione,
                    DataScadenza = cmd.DataScadenza
                };
                _dbContext.Tasks.Add(task);
                await _dbContext.SaveChangesAsync();

                return task.Id;
            }
        }

        public async Task<string> Handle(ChangeTaskStatusCommand cmd)
        {
            var statiValidi = new[] { "InAttesa", "InLavorazione", "Completato", "Validato", "Respinto" };

            if (!statiValidi.Contains(cmd.Stato))
            {
                throw new ArgumentException($"Stato non valido. I valori permessi sono: {string.Join(", ", statiValidi)}");
            }

            var task = await _dbContext.Tasks.FindAsync(cmd.Id);
            if (task == null)
            {
                throw new ArgumentException("Task non trovato.");
            }

            task.Stato = cmd.Stato;
            _dbContext.Tasks.Update(task);
            await _dbContext.SaveChangesAsync();

            return task.Stato;
        }

        public async Task<string> Handle(DeleteTaskCommand cmd)
        {
            var task = await _dbContext.Tasks.FindAsync(cmd.IdTask);
            if (task == null)
            {
                throw new ArgumentException("Task non trovato.");
            }

            var user = await _dbContext.Users.FindAsync(cmd.IdUtente);
            if (user == null)
            {
                throw new ArgumentException("Utente non trovato.");
            }

            bool autorized =
                (task.Tipologia == "Interno" && user.Ruolo == "ResponsabileInterno") ||      // !!! da rivedere i titoli dei responsabili (interno-esterno)!!!
                (task.Tipologia == "Esterno" && user.Ruolo == "ResponsabileEsterno");

            if (!autorized)
            {
                throw new UnauthorizedAccessException("Non sei autorizzato a eliminare questo task.");
            }
            else
            {
                _dbContext.Tasks.Remove(task);
                await _dbContext.SaveChangesAsync();
            }
            return $"Task con ID {task.Id} eliminato con successo."; // Messaggio di conferma
        }

        public async Task<string> Handle(PresaInCaricoTask cmd)
        {
            var task = await _dbContext.Tasks.FindAsync(cmd.TaskId);
            if (task == null)
            {
                throw new ArgumentException("Task non trovato.");
            }
            var user = await _dbContext.Users.FindAsync(cmd.IdAssegnatario);
            if (user == null)
            {
                throw new ArgumentException("Utente non trovato.");
            }

            if (task.Stato != "InAttesa")
            {
                throw new InvalidOperationException("Il task può essere preso in carico solo se è nello stato 'In Attesa'.");
            }

            task.Stato = "InLavorazione";
            task.IdAssegnatario = cmd.IdAssegnatario;

            _dbContext.Tasks.Update(task);
            await _dbContext.SaveChangesAsync();

            return $"Task con titolo \"{task.Titolo}\" è stato preso in carico da {user.Username}.";
        }

        public async Task<string> Handle(MarkTaskAsCompleted cmd)
        {
            var task = await _dbContext.Tasks.FindAsync(cmd.TaskId);
            if (task == null)
            {
                throw new ArgumentException("Task non trovato.");
            }

            if (task.IdAssegnatario == null || task.IdAssegnatario != cmd.IdUtente)
            {
                throw new UnauthorizedAccessException("Non sei autorizzato a segnare questo task come completato.");
            }

            task.Stato = "Completato";
            _dbContext.Tasks.Update(task);
            await _dbContext.SaveChangesAsync();

            return $"Task con titolo {task.Titolo} è stato segnato come completato.";

        }
    }
}
