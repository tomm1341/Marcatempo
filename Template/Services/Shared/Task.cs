using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Template.Services.Shared
{
    internal class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid IdCommessa { get; set;} //FK

        public String Stato { get; set;} //In lavorazione, completato ecc...
        public String Titolo { get; set;}
        public String Descrizione { get; set;}


    }
}
