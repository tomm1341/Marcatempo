using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Template.Services.Shared;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Template.Web.Areas;
using Template.Services;

namespace Template.Web.Features.Storico
{
    public partial class StoricoController : AuthenticatedBaseController
    {
        private readonly TemplateDbContext _dbContext;
        private readonly SharedService _sharedService;

        public StoricoController(TemplateDbContext dbContext, SharedService sharedService)
        {
            _dbContext = dbContext;
            _sharedService = sharedService;
        }

        [HttpGet]
        public async virtual Task<IActionResult> Storico()
        {
            var tasksCompletati = await _dbContext.Tasks
                .Where(task => task.Stato == "Completato" || task.Stato == "Approvato")
                .Select(task => new StoricoViewModel
                {
                    Id = task.Id,
                    Titolo = task.Titolo,
                    Stato = task.Stato,
                    Descrizione = task.Descrizione,
                    Tipologia = task.Tipologia == "Interna"
                                   ? TipologiaEvento.Interna
                                   : TipologiaEvento.Esterna,
                    Priorità = task.Priorità,
                    DataScadenza = task.DataScadenza
                })
                .ToListAsync();

            if (!tasksCompletati.Any())
                return View(tasksCompletati); // Mostra la view con “Nessun task”

            return View(tasksCompletati);
        }

        // Azione per approvare
        [HttpPost, ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> Approva(Guid id)
        {
            if (!(User.IsInRole("ResponsabileInterno") || User.IsInRole("ResponsabileEsterno")))
                return Forbid();

            await _sharedService.Handle(new ChangeTaskStatusCommand
            {
                Id = id,
                Stato = "Approvato"
            });

            TempData["Success"] = "Task approvato correttamente.";
            return RedirectToAction(nameof(Storico));
        }

        // Azione per respingere
        [HttpPost, ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> Respingi(Guid id)
        {
            if (!(User.IsInRole("ResponsabileInterno") || User.IsInRole("ResponsabileEsterno")))
                return Forbid();

            await _sharedService.Handle(new ChangeTaskStatusCommand
            {
                Id = id,
                Stato = "Respinto"
            });

            TempData["Success"] = "Task respinto correttamente.";
            return RedirectToAction(nameof(Storico));
        }
    }
}
