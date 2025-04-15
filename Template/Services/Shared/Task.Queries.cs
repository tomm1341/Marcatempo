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
        public Guid IdCreatore { get; set; } 
        public string Titolo { get; set; }
        public string Stato { get; set; }
        public string Tipologia { get; set; }

        public string Descrizione { get; set; }
        public DateTime DataCreazione { get; set; }
        public DateTime? DataScadenza { get; set; }
    }
    
    public partial class SharedService
    {
        public async Task<List<GetAllTasksQuery>> GetAllTasksAsync() // Returns every task without id
        {
            return await _dbContext.Tasks
                .OrderByDescending(t => t.DataCreazione)
                .Select(t => new GetAllTasksQuery
                {
                    IdCreatore = t.IdCreatore, // Valutare se IdCreatore oppure niente oppure Nome Creatore
                    Titolo = t.Titolo,
                    Stato = t.Stato,
                    Tipologia = t.Tipologia,
                    Descrizione = t.Descrizione,
                    DataCreazione = t.DataCreazione,
                    DataScadenza = t.DataScadenza
                })
                .ToListAsync();
        }
    }
    
}
