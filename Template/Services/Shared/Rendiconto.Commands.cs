using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Services.Shared
{
    public class RendicontoDTO
    {
        public Guid IdRendiconto { get; set; }
        public Guid IdUtente { get; set; }
        public Guid IdTask { get; set; }
        public int OreLavorate { get; set; }
        public DateTime Data { get; set; }  // Da valutare se prendere la data al completamento del task o data di presa in carico (per ora compeltamento)
        public int OraInizio { get; set; }
        public int OraFine { get; set; }

    }


    public partial class SharedService
    {
        public async Task<Guid> Handle(RendicontoDTO dto, Guid IdUtente, Guid IdTask, int OraInizio, int OraFine)
        {
            if (OraInizio < OraFine)
                throw new ArgumentException("L'ora di inizio non può essere precedente all'ora di fine");

            if (dto.Data.Date > DateTime.Today)
                throw new ArgumentException("La data di completamento non può essere futura");

            /*if (dto.OreLavorate <= 0)
                throw new ArgumentException("Errore nell'assegnazione del monteore");*/

            var rendiconto = new Rendiconto
            {
                Id = Guid.NewGuid(),
                IdUtente = IdUtente,
                IdTask = IdTask,
                OreLavorate = dto.OraFine - dto.OraInizio,
                Data = DateTime.Today,
                OraInizio = OraInizio,
                OraFine = OraFine
            };

            _dbContext.Rendiconto.Add(rendiconto);
            await _dbContext.SaveChangesAsync();

            return rendiconto.Id;

        }











    }

}
