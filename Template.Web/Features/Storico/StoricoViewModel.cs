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
        public string Descrizione { get; set; }   // Descrizione dell'evento
        public TipologiaEvento Tipologia { get; set; }  // Tipologia dell'evento (Interna o Esterna)
        public string Priorità { get; set; }   // Priorità dell'evento
        public List<Event> Events { get; set; } = new List<Event>();  // Lista di eventi associati
    }
}
    