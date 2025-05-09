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

            // Carico il VM di base
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
                IsOwner = dto.IdAssegnatario.HasValue && dto.IdAssegnatario.Value == CurrentUserId
            };

            if (vm.IsOwner)
            {
                var rendList = await _sharedService.GetRendicontoByTaskAsync(id);
                var last = rendList
                    .Where(r => r.IdUtente == CurrentUserId)
                    .OrderBy(r => r.Data).ThenBy(r => r.OraInizio)
                    .LastOrDefault();

                if (last != null)
                {
                    vm.Data = last.Data;
                    vm.OraInizio = last.OraInizio;
                    vm.OraFine = last.OraFine;
                }
            }

            return View("~/Features/Dettagli/Dettagli.cshtml", vm);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> SaveDetails(DettagliViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Details", model);

            if (model.IdAssegnatario != CurrentUserId)
                return Forbid();

            // Prepara DTO solo se ci sono dati di rendiconto
            RendicontoDTO rendDto = null;
            if (model.Data != default && model.OraFine > model.OraInizio)
            {
                rendDto = new RendicontoDTO
                {
                    IdUtente = CurrentUserId,
                    IdTask = model.TaskId,
                    Data = model.Data,
                    OraInizio = model.OraInizio,
                    OraFine = model.OraFine
                };
            }

            // CHIAMATA ORCHESTRATORE: nota il quarto parametro = CurrentUserId
            var newRendId = await _sharedService.UpdateTaskAndRendicontoAsync(
                model.TaskId,
                model.Descrizione,
                rendDto);

            TempData["Success"] = newRendId.HasValue
                ? "Descrizione e ore salvate."
                : "Descrizione salvata.";

            return RedirectToAction(nameof(Details), new { id = model.TaskId });
        }


    }

}


