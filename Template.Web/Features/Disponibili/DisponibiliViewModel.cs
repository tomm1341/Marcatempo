using System;

namespace Template.Web.Features.Disponibili
{
    public enum TipologiaEvento
    {
        Interna,
        Esterna
    }

    public enum PrioritaEvento
    {
        Alta,
        Media,
        Bassa
    }

    public class DisponibiliViewModel
    {
        public Guid Id { get; set; } 
        public string Descrizione { get; set; }
        public TipologiaEvento Tipologia { get; set; }
        public PrioritaEvento Priorità { get; set; }
        public DateTime? Scadenza { get; set; }
    }
}
