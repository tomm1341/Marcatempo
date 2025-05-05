using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Template.Services.Shared
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid IdCreatore { get; set;} // FK -> Utente

        public Guid? IdAssegnatario { get; set; } = null; // L'utente che prende in carico il task, inizialmente null

        [Required]
        public string Priorità { get; set;} // Alta, media, bassa

        [Required]
        [StatoValido(new[] { "InAttesa", "InLavorazione", "Completato" })]
        public string Stato { get; set; }
        public string Titolo { get; set;}

        [Required]
        [StatoValido(new[] {"Interno", "Esterno"})]
        public string Tipologia { get; set;} // Interno, esterno
        public string Descrizione { get; set;}

        public DateTime DataCreazione { get; set;} // Optional

        public DateTime DataScadenza { get; set;} // Optional


    }
    //********** Validazione Stato *************//
    public class StatoValidoAttribute : ValidationAttribute
    {
        private readonly string[] _valoriPermessi;

        public StatoValidoAttribute(string[] valoriPermessi)
        {
            _valoriPermessi = valoriPermessi;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !_valoriPermessi.Contains(value.ToString()))
            {
                return new ValidationResult($"Valore non valido. I valori permessi sono: {string.Join(", ", _valoriPermessi)}");
            }

            return ValidationResult.Success;
        }
    }
    //**********************************************//
}
