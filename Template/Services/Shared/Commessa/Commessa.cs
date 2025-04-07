using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Template.Services.Shared
{
    public class Commessa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid IdResponsabile { get; set; }

        public string Titolo { get; set; }

        public string PresaInCarico { get; set; } // Utente che prende in carico la commessa

        public string Stato { get; set; } // Stato della commessa, considerare sostituzione con "InLavorazione" booleano

    }
}
