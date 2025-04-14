using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Template.Services.Shared;
using System.Linq;
using System.Threading.Tasks;
using Template.Services;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Template.Web.Features.Storico
{
    public partial class StoricoController : Controller
    {
        private readonly TemplateDbContext _dbContext;

        public StoricoController(TemplateDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async virtual Task<IActionResult> Storico()
        {
            //var userEmail = User.Identity.Name; // Get user email

            //if (string.IsNullOrEmpty(userEmail))
            //{
            //    Console.WriteLine("User.Identity.Name è null o vuoto");
            //}
            //else
            //{
            //    Console.WriteLine($"User.Identity.Name: {userEmail}");
            //}

            //var user = await _dbContext.Users
            //    .Where(x => x.Email == userEmail)
            //    .Select(x => x.Id)
            //    .FirstOrDefaultAsync();

            //if (user == null)
            //{
            //    return NotFound("Utente non trovato");
            //}

            //// Retrieves only the completed user task from history
            //var allData = await _dbContext.Users
            //    .Where(x => x.Email == userEmail)
            //    .Select(x => new
            //    {
            //        x.Id,
            //        x.FirstName,
            //        x.Role,
            //        Events = x.Events
            //            .Where(e => e.Stato == "Completato")  // Filter events where state is "Completato"
            //            .Select(e => new
            //            {
            //                e.Descrizione,
            //                e.Tipologia,                          
            //                e.Stato
            //            })
            //            .ToList()
            //    })
            //    .ToListAsync();

            //if (allData == null || !allData.Any())
            //{
            //    return NotFound("Nessun task completato trovato.");
            //}

            //// Create the template for the view
            //var model = allData.Select(userData => new StoricoViewModel
            //{
            //    Nome = userData.FirstName,               
            //    Ruolo = userData.Role,
            //    Email = userEmail,               
            //    Events = userData.Events.Select(e => new Event
            //    {
            //        Tipologia = e.Tipologia,
            //        Descrizione = e.Descrizione,
            //        Stato = e.Stato
            //    }).ToList()
            //}).ToList();

            List<StoricoViewModel> list = [new() { Descrizione = "Task 1", Tipologia = TipologiaEvento.Interna }, new() { Descrizione = "Task 2", Tipologia = TipologiaEvento.Esterna }];

            return View(list);
        }
    }
}
