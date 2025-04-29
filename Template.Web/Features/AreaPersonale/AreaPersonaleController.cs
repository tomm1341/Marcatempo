using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template.Web.Features.AreaPersonale
{
    public partial class AreaPersonaleController : Controller
    {
        [HttpGet]
        public virtual IActionResult AreaPersonale()

        {
            // Formattazione per la visualizzazione del ruolo Responsabile
            var formattedRole = "";
            var tempRole = Identita.RuoloUtenteCorrente;
            if (tempRole == "ResponsabileInterno")
            {
                formattedRole = "Responsabile Interno";
            }
            else if (tempRole == "ResponsabileEsterno")
            {
                formattedRole = "Responsabile Esterno";
            }
            else formattedRole = tempRole;

                var model = new AreaPersonaleViewModel
                {
                    Nome = Identita.NomeUtenteCorrente,
                    Cognome = Identita.CognomeUtenteCorrente,
                    Ruolo = formattedRole,
                    TaskInLavorazione = new List<TaskItemViewModel>
                {
                    new() { Descrizione = "Fix bug login", Scadenza = DateTime.Today.AddDays(2), StatoColore = "green" },
                    new() { Descrizione = "Update landing page", Scadenza = DateTime.Today.AddDays(1), StatoColore = "yellow" },
                    new() { Descrizione = "Scrivere test coverage", Scadenza = DateTime.Today.AddDays(4), StatoColore = "gray" },
                },
                    RendicontoSettimana = new List<RendicontoGiornaliero>
                {
                    new() { Giorno = "Lunedì", Data = "21/04/25", OrarioInizio = "09:00", OrarioFine = "17:00" },
                    new() { Giorno = "Martedì", Data = "22/04/25", OrarioInizio = "09:15", OrarioFine = "17:15" },
                    new() { Giorno = "Mercoledì", Data = "23/04/25", OrarioInizio = "09:00", OrarioFine = "17:00" },
                    new() { Giorno = "Giovedì", Data = "24/04/25", OrarioInizio = "09:30", OrarioFine = "17:30" },
                    new() { Giorno = "Venerdì", Data = "25/04/25", OrarioInizio = "09:00", OrarioFine = "16:00" },
                },
                    OreTotali = 38
                };

            return View(model);
        }
    }
}
