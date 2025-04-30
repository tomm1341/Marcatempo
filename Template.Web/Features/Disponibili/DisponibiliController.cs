using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template.Web.Areas;
using Template.Web.Features.Task;
using Template.Web.Infrastructure;
using Template.Services;
using System.Linq;
using Template.Services.Shared;

namespace Template.Web.Features.Disponibili
{
    
    public partial class DisponibiliController : AuthenticatedBaseController
    {
        private readonly TemplateDbContext _dbContext;

        public DisponibiliController(TemplateDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Disponibili()
        {
            var tasks = await GetAvailableTasksAsync();

            var list = new List<DisponibiliViewModel>();

            foreach (var t in tasks)
            {
                Enum.TryParse(t.Tipologia, true, out TipologiaEvento tipologia);
                Enum.TryParse(t.Priorità, true, out PrioritaEvento priorita);

                list.Add(new DisponibiliViewModel
                {
                    Id = Guid.NewGuid(), // Se hai un ID reale da usare, sostituiscilo
                    Descrizione = t.Descrizione,
                    Tipologia = tipologia,
                    Priorità = priorita,
                    Scadenza = t.DataScadenza
                });
            }

            return View(list);
        }

        private async Task<IEnumerable<GetAvailableTasksQuery>> GetAvailableTasksAsync()
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
                    DataScadenza = t.DataScadenza,
                    IdCreatore = t.IdCreatore,
                    DataCreazione = t.DataCreazione
                })
                .ToListAsync();
        }
    }
}
