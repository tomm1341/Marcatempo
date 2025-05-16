using System;

namespace Template.Services.Shared
{
    // Modello che rappresenta le ferie o permessi nel sistema
    public class FeriePermesso
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Data { get; set; }
        public string Tipo { get; set; } // "Ferie" o "Permesso"
    }

    // DTO che usiamo per trasferire i dati dal controller
    public class FeriePermessoDTO
    {
        public Guid UserId { get; set; }
        public DateTime Data { get; set; }
        public string Tipo { get; set; }
    }
}
