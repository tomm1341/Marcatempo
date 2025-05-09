using System;
using System.Collections.Generic;

namespace Template.Web.Features.AreaPersonale
{
    public class TaskItemViewModel
    {
        public Guid Id { get; set; }
        public Guid IdCreatore { get; set; }
        public string Titolo { get; set; }
        public string Tipologia { get; set; }
        public string Stato { get; set; }
        public string Priorità { get; set; }
        public string Descrizione { get; set; }
        public DateTime DataCreazione { get; set; }
        public DateTime DataScadenza { get; set; }
        public string StatoColore { get; set; } // bootstrap color: "warning", "success", ecc.
    }

    /// <summary>
    /// ViewModel per ogni riga di rendiconto nella tabella in AreaPersonale.
    /// </summary>
    public class RendicontoLogViewModel
    {
        public string Giorno { get; set; }         // es. “Lunedì”
        public string Data { get; set; }           // “dd/MM/yyyy”
        public string TitoloTask { get; set; }     // Descrizione o titolo del task
        public int OreLavorate { get; set; }
        public string OrarioInizio { get; set; }
        public string OrarioFine { get; set; }

    }

    public class AreaPersonaleViewModel
    {
        public Guid UserId { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Ruolo { get; set; }

        public List<TaskItemViewModel> TaskInLavorazione { get; set; }

        /// <summary>
        /// Lista di tutte le voci di rendiconto giorno per giorno con il task e le ore.
        /// </summary>
        public List<RendicontoLogViewModel> RendicontoLogs { get; set; }

        public int OreTotali { get; set; }
    }
}
