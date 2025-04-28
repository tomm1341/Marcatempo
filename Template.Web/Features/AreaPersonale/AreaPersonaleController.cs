using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template.Web.Areas;

namespace Template.Web.Features.AreaPersonale
{
    public partial class AreaPersonaleController : AuthenticatedBaseController
    {
        [HttpGet]
        public virtual IActionResult AreaPersonale()

        {
            var model = new AreaPersonaleViewModel
            {
                Nome = "Mario",
                Cognome = "Rossi",
                Ruolo = "Developer",
                TaskInLavorazione = new List<TaskItemViewModel>
                {
                    new() { Descrizione = "Fix bug login", Scadenza = DateTime.Today.AddDays(2), StatoColore = "green" },
                    new() { Descrizione = "Update landing page", Scadenza = DateTime.Today.AddDays(1), StatoColore = "yellow" },
                    new() { Descrizione = "Scrivere test coverage", Scadenza = DateTime.Today.AddDays(4), StatoColore = "gray" },
                },
                RendicontoSettimana = new List<RendicontoGiornaliero>
                {
                    new() { Giorno = "Lunedì", OrarioInizio = "09:00", OrarioFine = "17:00" },
                    new() { Giorno = "Martedì", OrarioInizio = "09:15", OrarioFine = "17:15" },
                    new() { Giorno = "Mercoledì", OrarioInizio = "09:00", OrarioFine = "17:00" },
                    new() { Giorno = "Giovedì", OrarioInizio = "09:30", OrarioFine = "17:30" },
                    new() { Giorno = "Venerdì", OrarioInizio = "09:00", OrarioFine = "16:00" },
                },
                OreTotali = 38
            };

            return View(model);
        }
    }
}
