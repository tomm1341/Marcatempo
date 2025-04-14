using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Services;
using Template.Services.Shared;

namespace Template.Web.Features.Disponibili
{
    public partial class DisponibiliController : Controller
    {
        private readonly TemplateDbContext _dbContext;

        public DisponibiliController(TemplateDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async virtual Task<IActionResult> Disponibili()
        {
            // Creazione della lista di task disponibili
            List<DisponibiliViewModel> list = new List<DisponibiliViewModel>
            {
                new DisponibiliViewModel
                {
                    Descrizione = "Task 1",
                    Tipologia = TipologiaEvento.Interna,
                    Priorità = PrioritaEvento.Alta,
                    Scadenza = new DateTime(2025, 4, 15) // esempio di data di scadenza
                },
                new DisponibiliViewModel
                {
                    Descrizione = "Task 2",
                    Tipologia = TipologiaEvento.Esterna,
                    Priorità = PrioritaEvento.Media,
                    Scadenza = new DateTime(2025, 4, 20) // esempio di data di scadenza
                }
            };

            // Stampa i task per verificarli (puoi rimuovere questa parte in produzione)
            foreach (var task in list)
            {
                Console.WriteLine($"Descrizione: {task.Descrizione}, Tipologia: {task.Tipologia}, Priorità: {task.Priorità}, Scadenza: {task.Scadenza?.ToString("dd/MM/yyyy")}");
            }

            // Restituisci la vista con la lista di task
            return View(list);
        }
    }
}
