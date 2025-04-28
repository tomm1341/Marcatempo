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

namespace Template.Web.Features.Dettagli
{
    public partial class DettagliController : AuthenticatedBaseController
    {
        private readonly TemplateDbContext _dbContext;

        public DettagliController(TemplateDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async virtual Task<IActionResult> Dettagli()
        {
            // Placeholder con dati finti
            List<DettagliViewModel> list = new()
            {
                new DettagliViewModel
                {
                    Descrizione = "Task 1",
                    Stato = "In Lavorazione",
                    Priorità = "Alta",
                    Scadenza = DateTime.Today.AddDays(3)
                },
                
            };

            return View(list);
        }
    }
}
