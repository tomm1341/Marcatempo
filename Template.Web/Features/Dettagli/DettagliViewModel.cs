using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Template.Services.Shared;

namespace Template.Web.Features.Dettagli
{
    public class DettagliViewModel
    {
        public string Descrizione { get; set; } // Descrizione dell'evento

        public string Priorità { get; set; } // Alta, Media o Bassa

        public DateTime Scadenza { get; set; } // Data di scadenza

        public string Stato { get; set; } // In Lavorazione o Completato
    }
}
