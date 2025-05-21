using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Web.Areas;
using Template.Web.Infrastructure;
using Template.Services.Shared;

namespace Template.Web.Features.Disponibili
{
    public partial class DisponibiliController : AuthenticatedBaseController
    {
        private readonly SharedService _sharedService;

        public DisponibiliController(SharedService sharedService)
        {
            _sharedService = sharedService;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Disponibili()
        {
            var tasks = await _sharedService.GetAvailableTasksAsync();

            var list = tasks
                .Select(t =>
                {
                    Enum.TryParse(t.Tipologia, true, out TipologiaEvento tipologia);
                    Enum.TryParse(t.Priorità, true, out PrioritaEvento priorita);
                    return new DisponibiliViewModel
                    {
                        Id = t.Id,
                        Titolo = t.Titolo,
                        Descrizione = t.Descrizione,
                        Tipologia = tipologia,
                        Priorità = priorita,
                        Scadenza = t.DataScadenza
                    };
                })
                .ToList();

            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> PrendiInCarico(Guid taskId)
        {
            // Prende l'utente corrente
            var userId = Identita.IdUtenteCorrente;
            Console.WriteLine($"Assegnatario: {Identita.IdUtenteCorrente}");


            if (userId == Guid.Empty)
            {
                return BadRequest("Utente corrente non valido.");
            }

            // Esegue il comando lato backend
            var message = await _sharedService.Handle(new PresaInCaricoTask
            {
                TaskId = taskId,
                IdAssegnatario = userId
            });

            TempData["Success"] = message;
            return RedirectToAction(nameof(Disponibili));
        }
    }
}
