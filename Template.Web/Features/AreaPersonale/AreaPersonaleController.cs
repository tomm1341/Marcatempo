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

        [HttpGet]
        public async virtual Task<IActionResult> AreaPersonale1()
        {
            var userId = Identita.IdUtenteCorrente;

            // 1) Prendo dal DB i task assegnati all'utente
            var assignedDtos = await _sharedService.Query(new AssignedTaskQuery
            {
                UserId = userId
            });

            // 2) Mappo i DTO sul ViewModel
            var taskItems = assignedDtos
                .Select(dto => new TaskItemViewModel
                {
                    Id = dto.Id,
                    Titolo = dto.Titolo,
                    Descrizione = dto.Descrizione,
                    DataScadenza = dto.DataScadenza,
                    Stato = dto.Stato,
                    StatoColore = dto.Stato switch
                    {
                        "InLavorazione" => "warning",
                        "Completato" => "success",
                        _ => "secondary"
                    }
                })
                .ToList();

            // 3) (Ancora mock) Rendiconto settimanale e ore totali
            var rendiconto = new List<RendicontoGiornaliero>
            {
                new() { Giorno="Lunedì",   Data="21/04/25", OrarioInizio="09:00", OrarioFine="17:00" },
                new() { Giorno="Martedì",  Data="22/04/25", OrarioInizio="09:15", OrarioFine="17:15" },
                new() { Giorno="Mercoledì",Data="23/04/25", OrarioInizio="09:00", OrarioFine="17:00" },
                new() { Giorno="Giovedì",  Data="24/04/25", OrarioInizio="09:30", OrarioFine="17:30" },
                new() { Giorno="Venerdì",  Data="25/04/25", OrarioInizio="09:00", OrarioFine="16:00" },
            };
            var totaleOre = rendiconto
                .Sum(r => (TimeSpan.Parse(r.OrarioFine) - TimeSpan.Parse(r.OrarioInizio)).TotalHours);

            // 4) Costruisco e ritorno il ViewModel
            var model = new AreaPersonaleViewModel
            {
                UserId = userId,
                Nome = Identita.NomeUtenteCorrente,
                Cognome = Identita.CognomeUtenteCorrente,
                Ruolo = FormatRole(Identita.RuoloUtenteCorrente),
                TaskInLavorazione = taskItems,
                RendicontoSettimana = rendiconto,
                OreTotali = (int)totaleOre
            };

            return View(model);
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

            // 2) Leggi rendiconto reale dal DB
            var rendDtos = await _sharedService.GetRendicontoByUserAsync(userId);
            var rendiconto = rendDtos.Select(r => new RendicontoGiornaliero
            {
                Giorno = r.Data.ToString("dddd", new System.Globalization.CultureInfo("it-IT")),
                Data = r.Data.ToString("dd/MM/yyyy"),
                OrarioInizio = r.OraInizio.ToString("00") + ":00",
                OrarioFine = r.OraFine.ToString("00") + ":00"
            }).ToList();

            var totaleOre = rendDtos.Sum(r => r.OreLavorate);
            var model = new AreaPersonaleViewModel
            {
                UserId = userId,
                Nome = Identita.NomeUtenteCorrente,
                Cognome = Identita.CognomeUtenteCorrente,
                Ruolo = FormatRole(Identita.RuoloUtenteCorrente),
                TaskInLavorazione = taskItems,
                RendicontoSettimana = rendiconto,
                OreTotali = (int)totaleOre
            };

            return View(model);

        }
    }
}
