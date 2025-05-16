using System;
using System.Threading.Tasks;
using Template.Services.Shared;

namespace Template.Services.Shared
{
    public partial class SharedService
    {
        // Metodo per gestire l'inserimento delle ferie/permessi
        public async Task<Guid> Handle(FeriePermessoDTO dto)
        {
            // Verifica che la data non sia futura
            if (dto.Data.Date > DateTime.Today)
                throw new ArgumentException("La data non può essere futura");

            // Verifica che il tipo sia valido ("Ferie" o "Permesso")
            if (dto.Tipo != "Ferie" && dto.Tipo != "Permesso")
                throw new ArgumentException("Tipo non valido. Deve essere 'Ferie' o 'Permesso'");

            // Crea il record delle ferie/permessi
            var feriePermesso = new FeriePermesso
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                Data = dto.Data,
                Tipo = dto.Tipo
            };

            // Aggiungi il nuovo record al database
            _dbContext.FeriePermesso.Add(feriePermesso);
            await _dbContext.SaveChangesAsync();

            return feriePermesso.Id;
        }
    }
}
