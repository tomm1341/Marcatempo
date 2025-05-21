using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Web.Features.Task
{
    public class TaskViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        public string Titolo { get; set; }

        public string Descrizione { get; set; }

        [Required]
        public string Tipologia { get; set; }

        [Required]
        public string Priorità { get; set; } 

 
        public DateTime? DataScadenza { get; set; }

        public string Stato { get; set; } = "DaFare";

        public DateTime DataCreazione { get; set; } = DateTime.Now;

        public Guid IdCreatore { get; set; }

        public void SetTask(Template.Services.Shared.Task task)
        {
            Id = task.Id;
            Titolo = task.Titolo;
            Descrizione = task.Descrizione;
            Tipologia = task.Tipologia;
            Priorità = task.Priorità.ToString();
            Stato = task.Stato;
            DataScadenza = task.DataScadenza;
            DataCreazione = task.DataCreazione;
            IdCreatore = task.IdCreatore;
        }

    
    }
}