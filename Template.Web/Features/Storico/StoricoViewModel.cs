using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Template.Services.Shared;

namespace Template.Web.Features.Storico
{
    // Enum per definire i tipi di tipologia
    public enum TipologiaEvento
    {
        Interna,  // Evento interno
        Esterna   // Evento esterno
    }

    public class StoricoViewModel
    {
        public Guid Id { get; set; }

        public string Stato { get; set; }
        public string Titolo { get; set; }
        public string Descrizione { get; set; }
        public TipologiaEvento Tipologia { get; set; }
        public string Priorità { get; set; }
        public DateTime DataScadenza { get; set; }
    }
}
    