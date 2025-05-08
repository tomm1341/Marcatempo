using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Template.Services.Shared
{
    public partial class SharedService
    {
        public async Task<IEnumerable<RendicontoDTO>> GetRendicontoByUserAsync(Guid userId)
        {
            return await _dbContext.Rendiconto
                .Where(r => r.IdUtente == userId)
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
    }
}
