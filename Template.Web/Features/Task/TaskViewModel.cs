using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Template.Services.Shared;
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
        public DateTime DataScadenza { get; set; }

        public string Stato { get; set; } = "InAttesa";

        public DateTime DataCreazione { get; set; } = DateTime.Now;

        public Guid IdCreatore { get; set; }

        public void SetTask(Template.Services.Shared.Task task)
        {
            Id = task.Id;
            Titolo = task.Titolo;
            Descrizione = task.Descrizione;
            DataScadenza = task.DataScadenza;
            Stato = task.Stato;
            IdCreatore = task.IdCreatore;
            DataCreazione = task.DataCreazione;
        }
    }
}

//        public AddOrUpdateTaskCommand ToAddOrUpdateTaskCommand()
//        {
//            return new AddOrUpdateTaskCommand
//            {
//                Id = this.Id,
//                Titolo = this.Titolo,
//                Descrizione = this.Descrizione,
//                Stato = this.Stato,
//                DataScadenza = this.DataScadenza,
//                IdCreatore = this.IdCreatore,
//                DataCreazione = this.DataCreazione
//            };
//        }
//    }
//}
