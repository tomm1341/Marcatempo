using System;

namespace Template.Web.Features.Disponibili
{
    public enum TipologiaEvento
    {
        Interno,
        Esterno
    }

    public enum PrioritaEvento
    {
        Bassa = 0,
        Media = 1,
        Alta = 2
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
