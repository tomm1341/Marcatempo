using System.Collections.Generic;
using System;

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

    public class RendicontoGiornaliero
    {
        public string Giorno { get; set; }
        public string Data { get; set; } // DA CAMBIARE IL TIPO IN DateTime
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
        public List<RendicontoGiornaliero> RendicontoSettimana { get; set; }
        public int OreTotali { get; set; }
    }
}
