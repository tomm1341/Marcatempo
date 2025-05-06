using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Template.Services.Shared;

namespace Template.Web.Features.Dettagli
{
    public class DettagliViewModel
    {
        public Guid TaskId { get; set; }
        public Guid IdCreatore { get; set; }
        public string NomeCreatore { get; set; }
        public Guid? IdAssegnatario { get; set; }
        public string Titolo { get; set; }
        public string Tipologia { get; set; }
        public string Descrizione { get; set; }

        public string Priorità { get; set; } 
        public DateTime Creazione { get; set; }
        public DateTime Scadenza { get; set; } 
        public string Stato { get; set; } 
        public string OreLavorate { get; set; }

        public bool IsOwner { get; set; } // Utilizzato per gestione view
    }
}
