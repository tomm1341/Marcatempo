using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Services.Shared
{
    public class GetAllTasksQuery
    {
        public string Titolo { get; set; }
        public string Stato { get; set; }
        public string Tipologia { get; set; }

        public string Priorità { get; set; }
        public string Descrizione { get; set; }
        public DateTime DataCreazione { get; set; }
        public DateTime? DataScadenza { get; set; }
    }

    public class GetAvailableTasksQuery
    {
        public string Titolo { get; set; }
        public string Tipologia { get; set; }
        public string Stato { get; set; }
        public string Priorità { get; set; }
        public string Descrizione { get; set; }
        public DateTime DataScadenza { get; set; }

    }

    public class  GetCompletedTasksQuery
    {
        public string Titolo { get; set; }
        public string Tipologia { get; set; }
        public string Stato { get; set; }
        public string Descrizione { get; set; }
        public Guid IdAssegnatario { get; set; }
        public string NomeAssegnatario { get; set; }
    }

    public partial class SharedService
    {
        public async Task<IEnumerable<GetAllTasksQuery>> GetAllTasksAsync()
        {
            return await _dbContext.Tasks
                .OrderByDescending(t => t.DataCreazione)
                .Select(t => new GetAllTasksQuery
                {
                    Titolo = t.Titolo,
                    Stato = t.Stato,
                    Tipologia = t.Tipologia,
                    Descrizione = t.Descrizione,
                    Priorità = t.Priorità.ToString(),
                    DataCreazione = t.DataCreazione,
                    DataScadenza = t.DataScadenza
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<GetAvailableTasksQuery>> GetAvailableTasksAsync() 
        {
            return await _dbContext.Tasks
                .Where(t => t.Stato == "InAttesa")
                .Select(t => new GetAvailableTasksQuery
                {
                    Titolo = t.Titolo,
                    Priorità = t.Priorità.ToString(),
                    Stato = t.Stato,
                    Tipologia = t.Tipologia,
                    Descrizione = t.Descrizione,
                    DataScadenza = t.DataScadenza
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<GetCompletedTasksQuery>> GetCompletedTasksAsync()
        {
            return await _dbContext.Tasks
                .Where(t => t.Stato == "Completato" && t.IdAssegnatario != null)
                .Join(_dbContext.Users,
                      task => task.IdAssegnatario,
                      user => user.Id,
                      (task, user) => new GetCompletedTasksQuery
                      {
                          Titolo = task.Titolo,
                          Tipologia = task.Tipologia,
                          Stato = task.Stato,
                          Descrizione = task.Descrizione,
                          IdAssegnatario = user.Id,
                          NomeAssegnatario = $"{user.Nome} {user.Cognome}" // Merge di nome e cognome in un solo campo per comodità
                      })
                .ToListAsync();
        }
    }
}
