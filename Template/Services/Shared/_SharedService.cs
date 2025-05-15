using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Template.Services.Shared;

namespace Template.Services.Shared
{
    public partial class SharedService
    {
        private readonly TemplateDbContext _dbContext;

        public SharedService(TemplateDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Metodo per aggiornare un task e un rendiconto
        public async virtual Task<Guid?> UpdateTaskAndRendicontoAsync(
            Guid taskId,
            string nuovaDescrizione,
            RendicontoDTO rendDto)
        {
            var task = await _dbContext.Tasks.FindAsync(taskId);
            if (task == null)
                throw new ArgumentException("Task non trovato");
            task.Descrizione = nuovaDescrizione;
            _dbContext.Tasks.Update(task);

            Guid? rendId = null;

            if (rendDto != null
                && rendDto.OraFine > rendDto.OraInizio
                && rendDto.Data.Date <= DateTime.Today)
            {
                var existing = await _dbContext.Rendiconto
                    .FirstOrDefaultAsync(r =>
                        r.IdTask == taskId &&
                        r.IdUtente == rendDto.IdUtente &&
                        r.Data == rendDto.Data.Date);

                if (existing != null)
                {
                    existing.OraInizio = rendDto.OraInizio;
                    existing.OraFine = rendDto.OraFine;
                    existing.OreLavorate = rendDto.OraFine - rendDto.OraInizio;
                    _dbContext.Rendiconto.Update(existing);
                    rendId = existing.Id;
                }
                else
                {
                    var rend = new Rendiconto
                    {
                        Id = Guid.NewGuid(),
                        IdUtente = rendDto.IdUtente,
                        IdTask = taskId,
                        Data = rendDto.Data.Date,
                        OraInizio = rendDto.OraInizio,
                        OraFine = rendDto.OraFine,
                        OreLavorate = rendDto.OraFine - rendDto.OraInizio
                    };
                    _dbContext.Rendiconto.Add(rend);
                    rendId = rend.Id;
                }
            }

            await _dbContext.SaveChangesAsync();
            return rendId;
        }

        // Metodo per recuperare i rendiconti di un task
        public async virtual Task<IEnumerable<RendicontoDTO>> GetRendicontoByTaskAsync(Guid taskId)
        {
            return await _dbContext.Rendiconto
                .Where(r => r.IdTask == taskId)
                .Select(r => new RendicontoDTO
                {
                    IdRendiconto = r.Id,
                    IdUtente = r.IdUtente,
                    IdTask = r.IdTask,
                    OreLavorate = r.OreLavorate,
                    Data = r.Data,
                    OraInizio = r.OraInizio,
                    OraFine = r.OraFine
                })
                .ToListAsync();
        }

        // Nuovo metodo per aggiornare o salvare ferie/permessi
        public async virtual Task<Guid?> UpdateFeriePermessoAsync(Guid userId, DateTime data, string tipo)
        {
            var feriePermesso = await _dbContext.FeriePermesso
                .FirstOrDefaultAsync(fp => fp.UserId == userId && fp.Data.Date == data.Date);

            if (feriePermesso != null)
            {
                // Se esiste già, aggiorna
                feriePermesso.Tipo = tipo;
                _dbContext.FeriePermesso.Update(feriePermesso);
            }
            else
            {
                // Se non esiste, crea un nuovo record
                feriePermesso = new FeriePermesso
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Data = data,
                    Tipo = tipo  // "Ferie" o "Permesso"
                };
                _dbContext.FeriePermesso.Add(feriePermesso);
            }

            await _dbContext.SaveChangesAsync();
            return feriePermesso.Id;
        }

        // Nuovo metodo per recuperare tutte le ferie/permessi di un utente
        public async virtual Task<IEnumerable<FeriePermessoDTO>> GetFeriePermessiByUserAsync(Guid userId)
        {
            return await _dbContext.FeriePermesso
                .Where(fp => fp.UserId == userId)
                .Select(fp => new FeriePermessoDTO
                {
                    UserId = fp.UserId,
                    Data = fp.Data,
                    Tipo = fp.Tipo // "Ferie" o "Permesso"
                })
                .ToListAsync();
        }
    }
}
