using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Template.Services.Shared
{
    internal class Commessa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid IdResponsabile { get; set; }

        public String PresaInCarico { get; set; } = null; // Utente che prende in carico la commessa

        public String Stato { get; set; } // Stato della commessa, considerare sostituzione con "InLavorazione" booleano

    }
}
