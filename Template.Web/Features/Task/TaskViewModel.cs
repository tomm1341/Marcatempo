using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Template.Services.Shared;

namespace Template.Web.Features.Task
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Descrizione { get; set; }
        public string Tipologia { get; set; } // Interno o Esterno
        public string Priorità { get; set; } // Bassa, Media, Alta
        public DateTime Scadenza { get; set; }
        public bool Completo { get; set; } // Indica se il task è stato completato
        public string AssegnatoA { get; set; } // Nome del dipendente assegnato al task
    }

}
