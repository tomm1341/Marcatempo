using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Template.Services.Shared;
using System.Linq;
using System.Threading.Tasks;
using Template.Services;
using System;
using Template.Web.Areas;
using Template.Infrastructure;

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
        public async virtual Task<IActionResult> Details(Guid id, bool ritorna = false, bool respinto = false)
        {
            if (respinto)
            {
                // se vengo da "Respingi" con motivazione
                await _sharedService.Handle(new ChangeTaskStatusCommand
                {
                    Id = id,
                    Stato = "Respinto"
                });
                TempData["Success"] = "Task respinto. Inserisci qui sotto il motivo nella descrizione.";
            }
            else if (ritorna)
            {
                // se usavi ancora 'ritorna' per rimettere in lavorazione
                await _sharedService.Handle(new ChangeTaskStatusCommand
                {
                    Id = id,
                    Stato = "InLavorazione"
                });
                TempData["Success"] = "Task rimesso in lavorazione. Puoi ora specificare il motivo nella descrizione.";
            }

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
                NomeAssegnatario = dto.NomeAssegnatario,
                NomeCreatore = dto.NomeCreatore,
                IsOwner = dto.IdAssegnatario.HasValue && dto.IdAssegnatario.Value == CurrentUserId
            };

            if (dto.IdAssegnatario.HasValue)
            {
                vm.NomeAssegnatario =
                    await _sharedService.GetAssigneeNameByTaskAsync(id)
                    ?? "—";
            }
            else
            {
                vm.NomeAssegnatario = "Nessun assegnatario";
            }

            if (vm.IsOwner)
            {
                var rendList = await _sharedService.GetRendicontoByTaskAsync(id);
                var last = rendList
                    .Where(r => r.IdUtente == CurrentUserId)
                    .OrderBy(r => r.Data)
                    .ThenBy(r => r.OraInizio)
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
                return RedirectToAction(nameof(Details), new { id = model.TaskId });

            // ricava il ruolo corrente
            var identita = (IdentitaViewModel)ViewData[IdentitaViewModel.VIEWDATA_IDENTITACORRENTE_KEY];
            bool isResponsabile = identita.RuoloUtenteCorrente == "ResponsabileInterno"
                                || identita.RuoloUtenteCorrente == "ResponsabileEsterno";
            bool isOwner = model.IdAssegnatario == CurrentUserId;

            if (!isOwner && !isResponsabile)
                return Forbid();

            RendicontoDTO rendDto = null;
            if (model.Data != default && model.OraFine > model.OraInizio && isOwner)
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

            // aggiorna descrizione (e rendiconto se owner)
            var newRendId = await _sharedService.UpdateTaskAndRendicontoAsync(
                model.TaskId,
                model.Descrizione,
                rendDto);

            TempData["Success"] = newRendId.HasValue
                ? "Descrizione e ore salvate."
                : "Descrizione salvata.";

            return RedirectToAction(nameof(Details), new { id = model.TaskId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> MarkAsCompleted(Guid id)
        {
            var task = await _dbContext.Tasks.FindAsync(id);
            if (task == null)
            {
                TempData["Error"] = "Task non trovato.";
                return RedirectToAction(nameof(Details), new { id });
            }
            if (task.IdAssegnatario != CurrentUserId)
            {
                TempData["Error"] = "Non sei autorizzato a modificare questo task.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var hasRendiconto = await _dbContext.Rendiconto
                .AnyAsync(r => r.IdTask == id && r.IdUtente == CurrentUserId);
            if (!hasRendiconto)
            {
                TempData["Error"] = "Devi prima dichiarare le ore lavorate per poter completare il task.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var message = await _sharedService.Handle(new MarkTaskAsCompleted
            {
                TaskId = id,
                IdUtente = CurrentUserId
            });

            TempData["Success"] = message;
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
