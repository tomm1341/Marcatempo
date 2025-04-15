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
        public Guid Id { get; set; }

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
        public async Task<List<Task>> GetAllTasksAsync() // Returns every task
        {
            return await _dbContext.Tasks
                .OrderByDescending(t => t.DataCreazione)
                .ToListAsync();
        }
    }
    
}
