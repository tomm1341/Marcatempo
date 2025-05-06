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
using Template.Infrastructure;
using Microsoft.Extensions.Localization;
using Template.Web.Features.Login;

namespace Template.Web.Features.Dettagli
{
    public partial class DettagliController : AuthenticatedBaseController
    {
        private readonly TemplateDbContext _dbContext;
        private readonly SharedService _sharedService;

        public DettagliController(TemplateDbContext dbContext, SharedService sharedService)
        {
            _dbContext = dbContext;
            _sharedService = sharedService;
        }
            [HttpGet]
            public async virtual Task<IActionResult> Details(Guid id)
            {
                var dto = await _sharedService.Query(new TaskDetailQuery { Id = id });

                var vm = new DettagliViewModel
                {
                    TaskId = dto.Id,
                    Titolo = dto.Titolo,
                    Stato = dto.Stato,
                    Tipologia = dto.Tipologia,
                    Descrizione = dto.Descrizione,
                    Priorità = dto.Priorità,
                    Creazione = dto.DataCreazione,
                    Scadenza = dto.DataScadenza,
                    IdAssegnatario = dto.IdAssegnatario,
                    IdCreatore = dto.IdCreatore,
                    NomeCreatore = dto.NomeCreatore,

                    IsOwner = dto.IdAssegnatario.HasValue && dto.IdAssegnatario.Value == CurrentUserId // Controlla se l'utente è owner del task
                };

                return View("~/Features/Dettagli/Dettagli.cshtml", vm);
            }
        /* QUERY DI MODIFICA
            [HttpPost]
            public async Task<IActionResult> CompletaTask(DettagliViewModel model)
            {
                var task = await _dbContext.Tasks.FindAsync(model.TaskId);
                if (task == null) return NotFound();

                // Solo assegnatario può modificare
                if (task.IdAssegnatario != UserId) return Forbid();

                task.Descrizione = model.Descrizione;
                task.OreLavorate = model.OreLavorate;
                task.Stato = "Completato";

                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Details", new { id = task.Id });
            }*/

    }
}

