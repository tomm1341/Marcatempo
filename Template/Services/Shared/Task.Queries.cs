using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Services.Shared
{
    public class GetAllTasksQuery
    {
        public string Titolo { get; set; }
        public string Stato { get; set; }
        public string Tipologia { get; set; }

        public string Priorità { get; set; }
        public string Descrizione { get; set; }
        public DateTime DataCreazione { get; set; }
        public DateTime? DataScadenza { get; set; }
    }

    public class GetAvailableTasksQuery
    {
        public Guid Id { get; set; }
        public Guid IdCreatore { get; set; }
        public string Titolo { get; set; }
        public string Tipologia { get; set; }
        public string Stato { get; set; }
        public string Priorità { get; set; }
        public string Descrizione { get; set; }
        public DateTime DataCreazione { get; set; }  
        public DateTime? DataScadenza { get; set; }

    }

    public class  GetCompletedTasksQuery
    {
        public string Titolo { get; set; }
        public string Tipologia { get; set; }
        public string Stato { get; set; }
        public string Descrizione { get; set; }
        public Guid IdAssegnatario { get; set; }
        public string NomeAssegnatario { get; set; }
    }

    public class AssignedTaskQuery
    {
        public Guid UserId { get; set; }
    }

    public class AssignedTaskDTO
    {
        public Guid Id { get; set; }
        public Guid IdCreatore { get; set; }
        public string Titolo { get; set; }
        public string Tipologia { get; set; }
        public string Stato { get; set; }
        public string Priorità { get; set; }
        public string Descrizione { get; set; }
        public DateTime DataCreazione { get; set; }
        public DateTime? DataScadenza { get; set; }
    }

    public class TaskDetailQuery
    {
        public Guid Id { get; set; }
    }

    public class TaskDetailDTO
    {
        public Guid Id { get; set; }
        public Guid IdCreatore { get; set; }
        public string Titolo { get; set; }
        public string Stato { get; set; }
        public string Tipologia { get; set; }
        public string Descrizione { get; set; }
        public string Priorità { get; set; }
        public string NomeAssegnatario { get; set; }
        public string NomeCreatore { get; set; }
        public DateTime DataCreazione { get; set; }
        public DateTime? DataScadenza { get; set; }
        public Guid? IdAssegnatario { get; set; }
    }

    public class GetIntestatarioFromIdDTO
    {
        public Guid TaskId { get; set; }
        public Guid IdAssegnatario { get; set; }

        public class User {
            public Guid UserId { get; set; }
            public string Nome { get; set; }
            public string Cognome { get; set; }
        }
    }

        public class ChangeTaskDescriptionCommand
        {
            public Guid Id { get; set; }

            public string Descrizione { get; set; }

            public ChangeTaskDescriptionCommand(Guid id, string descrizione)
            {
                Id = id;
                Descrizione = descrizione;
            }
        }
    
      

    public partial class SharedService
    {
        public async Task<IEnumerable<GetAllTasksQuery>> GetAllTasksAsync()
        {
            return await _dbContext.Tasks
                .OrderByDescending(t => t.DataCreazione)
                .Select(t => new GetAllTasksQuery
                {
                    Titolo = t.Titolo,
                    Stato = t.Stato,
                    Tipologia = t.Tipologia,
                    Descrizione = t.Descrizione,
                    Priorità = t.Priorità,
                    DataCreazione = t.DataCreazione,
                    DataScadenza = t.DataScadenza
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<GetAvailableTasksQuery>> GetAvailableTasksAsync() 
        {
            return await _dbContext.Tasks
                .Where(t => t.Stato == "InAttesa")
                .Select(t => new GetAvailableTasksQuery
                {
                    Id = t.Id,
                    Titolo = t.Titolo,
                    Priorità = t.Priorità,
                    Stato = t.Stato,
                    Tipologia = t.Tipologia,
                    Descrizione = t.Descrizione,
                    DataScadenza = t.DataScadenza
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<GetCompletedTasksQuery>> GetCompletedTasksAsync()
        {
            return await _dbContext.Tasks
                .Where(t => t.Stato == "Completato" && t.IdAssegnatario != null)
                .Join(_dbContext.Users,
                      task => task.IdAssegnatario,
                      user => user.Id,
                      (task, user) => new GetCompletedTasksQuery
                      {
                          Titolo = task.Titolo,
                          Tipologia = task.Tipologia,
                          Stato = task.Stato,
                          Descrizione = task.Descrizione,
                          IdAssegnatario = user.Id,
                          NomeAssegnatario = $"{user.Nome} {user.Cognome}" // Merge di nome e cognome in un solo campo per comodità
                      })
                .ToListAsync();
        }
            /// <summary>
            /// Restituisce tutti i task il cui IdAssegnatario corrisponde all'utente passato
            /// </summary>
            public async Task<IEnumerable<AssignedTaskDTO>> Query(AssignedTaskQuery qry)
            {
                return await _dbContext.Tasks
                    .Where(t => t.IdAssegnatario == qry.UserId && t.Stato == "InLavorazione")
                    .Select(t => new AssignedTaskDTO
                    {
                        Id = t.Id,
                        IdCreatore = t.IdCreatore,
                        Titolo = t.Titolo,
                        Stato = t.Stato,
                        Tipologia = t.Tipologia,
                        Descrizione = t.Descrizione,
                        Priorità = t.Priorità,
                        DataCreazione = t.DataCreazione,
                        DataScadenza = t.DataScadenza
                    })
                    .ToListAsync();
            
        }
        public async Task<TaskDetailDTO> Query(TaskDetailQuery qry)
        {
            var t = await _dbContext.Tasks
                .Where(x => x.Id == qry.Id)
                .Select(x => new TaskDetailDTO
                {
                    Id = x.Id,
                    IdCreatore = x.IdCreatore,
                    Titolo = x.Titolo,
                    Stato = x.Stato,
                    Tipologia = x.Tipologia,
                    Descrizione = x.Descrizione,
                    Priorità = x.Priorità,
                    DataCreazione = x.DataCreazione,
                    DataScadenza = x.DataScadenza,
                    IdAssegnatario = x.IdAssegnatario,
                    NomeCreatore = _dbContext.Users
                                    .Where(u => u.Id == x.IdCreatore)
                                    .Select(u => u.Nome + " " + u.Cognome)
                                    .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (t == null) throw new ArgumentException("Task non trovato");
            return t;
        }

        public async Task<string> Handle(ChangeTaskDescriptionCommand cmd)
        {
            var task = await _dbContext.Tasks.FindAsync(cmd.Id);
            if (task == null)
                throw new KeyNotFoundException($"Task con Id {cmd.Id} non trovato.");

            task.Descrizione = cmd.Descrizione;
            _dbContext.Tasks.Update(task);
            await _dbContext.SaveChangesAsync();

            return task.Descrizione;
        }

        public async Task<string> GetAssigneeNameByTaskAsync(Guid taskId)
        {
            var fullName = await (
                from t in _dbContext.Tasks
                where t.Id == taskId && t.IdAssegnatario != null
                join u in _dbContext.Users
                    on t.IdAssegnatario equals u.Id
                select u.Nome + " " + u.Cognome
            ).FirstOrDefaultAsync();

            return fullName;
        }


    }
}
