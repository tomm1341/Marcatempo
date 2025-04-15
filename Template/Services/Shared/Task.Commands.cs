using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Services.Shared
{
    public class CreateTaskCommand // DTO (solo i campi che vengono passati dal frontend)
    {
        public int Priorità { get; set; }
        public string Stato { get; set; }
        public string Titolo { get; set; }
        public string Tipologia { get; set; }
        public string Descrizione { get; set; }
        public DateTime DataScadenza { get; set; }
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
                    Tipologia = cmd.Tipologia,
                    Descrizione = cmd.Descrizione,
                    DataCreazione = dataCreazione,
                    DataScadenza = cmd.DataScadenza
                };
                _dbContext.Tasks.Add(task);
                await _dbContext.SaveChangesAsync();

                return task.Id;
            }
        }
    }
}
