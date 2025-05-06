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
                    IdAssegnatario = dto.IdAssegnatario
                };

                return View("~/Features/Dettagli/Dettagli.cshtml", vm);
            }
        }
    }

