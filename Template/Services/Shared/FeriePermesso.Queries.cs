using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Template.Services.Shared
{
    public partial class SharedService
    {
        // Metodo per recuperare tutte le ferie/permessi per un utente
        public async Task<IEnumerable<FeriePermessoDTO>> GetFeriePermessoByUserAsync(Guid userId)
        {
            return await _dbContext.FeriePermesso
                .Where(fp => fp.UserId == userId)
                .Select(fp => new FeriePermessoDTO
                {
                    UserId = fp.UserId,
                    Data = fp.Data,
                    Tipo = fp.Tipo
                })
                .ToListAsync();
        }
    }
}
