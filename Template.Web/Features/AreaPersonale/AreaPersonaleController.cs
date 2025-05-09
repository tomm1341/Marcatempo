using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template.Web.Areas;
using Template.Services.Shared;
using Template.Web.Features.AreaPersonale;
using System.Collections.Generic;

namespace Template.Web.Features.AreaPersonale
{
    public partial class AreaPersonaleController : AuthenticatedBaseController
    {
        private readonly SharedService _sharedService;

        public AreaPersonaleController(SharedService sharedService)
        {
            _sharedService = sharedService;
        }


        private string FormatRole(string role) =>
            role switch
            {
                "ResponsabileInterno" => "Responsabile Interno",
                "ResponsabileEsterno" => "Responsabile Esterno",
                _ => role
            };

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> CompletaTask(Guid id)
        {
            // Ottieni l'ID utente corrente (opzionale, se vuoi loggare chi completa)
            var userId = Identita.IdUtenteCorrente;

            // Esegui il comando per cambiare stato
            var newState = await _sharedService.Handle(new ChangeTaskStatusCommand
            {
                Id = id,
                Stato = "Completato"
            });

            TempData["Success"] = $"Task {id} contrassegnato come '{newState}'.";

            return RedirectToAction(nameof(AreaPersonale));
        }

        [HttpGet]
        public async virtual Task<IActionResult> AreaPersonale()
        {
            var userId = Identita.IdUtenteCorrente;

            // 1) Task assegnati
            var assignedDtos = await _sharedService.Query(new AssignedTaskQuery { UserId = userId });
            var taskItems = assignedDtos.Select(dto => new TaskItemViewModel
            {
                Id = dto.Id,
                Descrizione = dto.Descrizione,
                DataScadenza = dto.DataScadenza,
                Stato = dto.Stato,
                StatoColore = dto.Stato switch
                {
                    "InLavorazione" => "warning",
                    "Completato" => "success",
                    _ => "secondary"
                }
            }).ToList();

            // 2) Rendiconti con Task associato
            var rendDtos = await _sharedService.GetRendicontoByUserAsync(userId);

            // Recupero anche i task per mappare i titoli nel rendiconto
            var allTasks = assignedDtos.ToDictionary(t => t.Id, t => t.Titolo);

            var rendicontoLogs = rendDtos.Select(r => new RendicontoLogViewModel
            {
                Giorno = r.Data.ToString("dddd", new System.Globalization.CultureInfo("it-IT")),
                Data = r.Data.ToString("dd/MM/yyyy"),
                OrarioInizio = r.OraInizio.ToString("00") + ":00",
                OrarioFine = r.OraFine.ToString("00") + ":00",
                TitoloTask = allTasks.ContainsKey(r.IdTask) ? allTasks[r.IdTask] : "Task sconosciuto",
                OreLavorate = r.OreLavorate
            }).OrderBy(r => DateTime.ParseExact(r.Data, "dd/MM/yyyy", null)).ToList();

            var totaleOre = rendDtos.Sum(r => r.OreLavorate);

            // 3) ViewModel finale
            var model = new AreaPersonaleViewModel
            {
                UserId = userId,
                Nome = Identita.NomeUtenteCorrente,
                Cognome = Identita.CognomeUtenteCorrente,
                Ruolo = FormatRole(Identita.RuoloUtenteCorrente),
                TaskInLavorazione = taskItems,
                RendicontoLogs = rendicontoLogs,
                OreTotali = (int)totaleOre
            };

            return View(model);
        }

    }
}
