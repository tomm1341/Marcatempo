using System;
using System.Threading.Tasks;
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

        // The following code is used to handle the 'Salva modifiche' button in Dettagli
        public async Task<Guid?> UpdateTaskAndRendicontoAsync(
            Guid taskId,
            string nuovaDescrizione,
            RendicontoDTO rendDto,
            Guid currentUserId)
        {
            var task = await _dbContext.Tasks.FindAsync(taskId);
            if (task == null)
                throw new ArgumentException("Task non trovato");
            if (task.IdAssegnatario != currentUserId)
                throw new UnauthorizedAccessException("Non sei autorizzato");
            task.Descrizione = nuovaDescrizione;
            _dbContext.Tasks.Update(task);

            Guid? nuovoRendId = null;
            if (rendDto != null
                && rendDto.OraFine > rendDto.OraInizio
                && rendDto.Data.Date <= DateTime.Today)
            {
                var rend = new Rendiconto
                {
                    Id = Guid.NewGuid(),
                    IdUtente = currentUserId,
                    IdTask = taskId,
                    OreLavorate = rendDto.OraFine - rendDto.OraInizio,
                    Data = rendDto.Data.Date,
                    OraInizio = rendDto.OraInizio,
                    OraFine = rendDto.OraFine
                };
                nuovoRendId = rend.Id;
                _dbContext.Rendiconto.Add(rend);
            }

            await _dbContext.SaveChangesAsync();

            return nuovoRendId;
        }
    }
}
