using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template.Web.Areas;
using Template.Services.Shared;
using Template.Web.Features.AreaPersonale;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Globalization;


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
                    "Respinto" => "danger",
                    _ => "secondary"
                }
            }).ToList();

            // 2) Rendiconti
            var rendDtos = await _sharedService.GetRendicontoByUserAsync(userId);

            // 2b) Mappa i rendiconti recuperando titolo via SharedService
            var rendicontoLogs = new List<RendicontoLogViewModel>();
            foreach (var r in rendDtos.OrderBy(r => r.Data))
            {
                // per ogni rendDto chiedo al servizio il dettaglio per ottenere il titolo
                var detail = await _sharedService.Query(new TaskDetailQuery { Id = r.IdTask });
                var titolo = detail?.Titolo ?? "Task non trovato";

                rendicontoLogs.Add(new RendicontoLogViewModel
                {
                    Giorno = r.Data.ToString("dddd", new System.Globalization.CultureInfo("it-IT")),
                    Data = r.Data.ToString("dd/MM/yyyy"),
                    OrarioInizio = r.OraInizio.ToString("00") + ":00",
                    OrarioFine = r.OraFine.ToString("00") + ":00",
                    OreLavorate = r.OreLavorate,
                    TitoloTask = titolo
                });
            }

            // Totale ore lavorate
            var totaleOre = rendDtos.Sum(r => r.OreLavorate);

            // 3) Ferie / Permessi
            var feriePermessiDtos = await _sharedService.GetFeriePermessiByUserAsync(userId);
            var feriePermessiLogs = feriePermessiDtos.Select(fp => new FeriePermessoLogViewModel
            {
                Data = fp.Data.ToString("dd/MM/yyyy"),
                Tipo = fp.Tipo
            }).ToList();

            // 4) Costruzione del ViewModel
            var model = new AreaPersonaleViewModel
            {
                UserId = userId,
                Nome = Identita.NomeUtenteCorrente,
                Cognome = Identita.CognomeUtenteCorrente,
                Ruolo = FormatRole(Identita.RuoloUtenteCorrente),
                TaskInLavorazione = taskItems,
                RendicontoLogs = rendicontoLogs,
                OreTotali = (int)totaleOre,
                FeriePermessoLogs = feriePermessiLogs
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> AggiungiFeriePermesso(DateTime dataInizio, DateTime dataFine, string tipo)
        {
            var userId = Identita.IdUtenteCorrente;

            if (dataFine < dataInizio)
            {
                TempData["Error"] = "La data fine non può essere precedente alla data inizio.";
                return RedirectToAction(nameof(AreaPersonale));
            }

            try
            {
                for (var giorno = dataInizio.Date; giorno <= dataFine.Date; giorno = giorno.AddDays(1))
                {
                    await _sharedService.UpdateFeriePermessoAsync(userId, giorno, tipo);
                }

                TempData["Success"] = $"{tipo} inserito per il periodo dal {dataInizio:dd/MM/yyyy} al {dataFine:dd/MM/yyyy}.";
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(AreaPersonale));
        }

    }
}
