using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Template.Services.Shared
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid IdCommessa { get; set;} //FK

        [Required]
        [StatoValido(new[] { "InAttesa", "InLavorazione", "Completato", "Approvato", "Respinto" })]
        public string Stato { get; set; }
        public string Titolo { get; set;}
        public string Descrizione { get; set;}


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
