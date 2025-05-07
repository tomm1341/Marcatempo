using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Template.Services.Shared;
using System.Linq;
using System.Threading.Tasks;
using Template.Services;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Template.Web.Areas;

namespace Template.Web.Features.Storico
{
    public partial class StoricoController : AuthenticatedBaseController
    {
        private readonly TemplateDbContext _dbContext;

        public StoricoController(TemplateDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Storico()
        {
            var tasksCompletati = await _dbContext.Tasks
                .Where(task => task.Stato == "Completato")
                .Select(task => new StoricoViewModel
                {
                    Id = task.Id,
                    Titolo = task.Titolo,
                    Stato = task.Stato,
                    Descrizione = task.Descrizione,
                    Tipologia = task.Tipologia == "Interna" ? TipologiaEvento.Interna : TipologiaEvento.Esterna,
                    Priorità = task.Priorità,
                    DataScadenza = task.DataScadenza
                })
                .ToListAsync();

            if (!tasksCompletati.Any())
            {
                return NotFound("Nessun task completato trovato.");
            }

            return View(tasksCompletati);
        }
    }
}
