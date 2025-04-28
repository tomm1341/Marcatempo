using Microsoft.Extensions.Logging;
using System;
using Template.Services.Shared;

namespace Template.Web.Features.Disponibili
{
    // Enum per definire i tipi di tipologia
    public enum TipologiaEvento
    {
        Interna,  // Evento interno
        Esterna   // Evento esterno
    }

    // Enum per definire le priorità degli eventi
    public enum PrioritaEvento
    {
        Alta,   // Alta priorità
        Media,  // Media priorità
        Bassa   // Bassa priorità
    }

    public class DisponibiliViewModel
    {
        public string Descrizione { get; set; }  // Descrizione dell'evento
        public TipologiaEvento Tipologia { get; set; }  // Tipologia dell'evento (Interna o Esterna)
        public PrioritaEvento Priorità { get; set; }  // Priorità dell'evento (Alta, Media, Bassa)

        public DateTime? Scadenza { get; set; }  // Scadenza dell'evento (opzionale)

        
    }
}
