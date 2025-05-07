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
        public Guid Id { get; set; }
        public Guid IdUtente { get; set; }
        public Guid IdTask { get; set; }

        public DateTime Data { get; set; } // Data del giorno corrente

        public int OreLavorate { get; set; } // Ore lavorate in un giorno

        public int OraInizio { get; set; } // Ora di inizio lavoro, da passare in automatico lato frontend

        public int OraFine { get; set; } // Ora di fine lavoro, da passare in automatico lato frontend


    }
}


/*
 VALUTARE LA CREAZIONE DI UN'ALTRA TABELLA RENDICONTO_SETTIMANALE (O MENSILE AD ESEMPIO). RENDICONTO NORMALE TIENE CONTO DELLE ORE LAVORATE IN UN GIORNO,
MENTRE QUELLO SETTIMANALE/MENSILE EFFETTUA IL CALCOLO DELLE ORE LAVORATE IN QUEL PERIODO, MAGARI ANCHE CALCOLANDO LA PAGA DOVUTA.
 */