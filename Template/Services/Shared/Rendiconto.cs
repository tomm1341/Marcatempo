using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Template.Services.Shared
{
    public class Rendiconto
    {
        [Key]
        public Guid IdUtente { get; set; }

        public Guid IdCommessa { get; set; }

        public Guid IdTask {  get; set; }

        public int Ore {  get; set; }

        public String Descrizione { get; set; }


    }
}
