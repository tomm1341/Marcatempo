using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Services.Shared
{
    public class ResocontoSettimanale
    {
        [Key]
        public Guid Id { get; set; }
        public Guid IdUtente { get; set; }
        [Required]
        public int NumeroSettimana { get; set; } // ISO week number
        public int OreTotaliLavorate { get; set; }
        public int GiorniLavorati { get; set; }
        [Required]
        public DateTime DataInizioSettimana { get; set; }
        [Required]
        public DateTime DataFineSettimana { get; set; }
    }

}


//da valutare bene la logica