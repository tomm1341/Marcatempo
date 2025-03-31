using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Template.Web.Features.Task
{
    public partial class TaskController : Controller
    {
        private static List<TaskViewModel> tasks = new List<TaskViewModel>();

        // Azione per visualizzare i task disponibili per i dipendenti
        [HttpGet]
        public IActionResult AvailableTasksForEmployees(string userRole, string employeeName)
        {
            var availableTasks = tasks.Where(t => !t.Completo && string.IsNullOrEmpty(t.AssegnatoA)).ToList();
            return View(availableTasks); // Mostra i task disponibili per essere assegnati
        }


        // Azione per assegnare un task a un dipendente
        [HttpPost]
        public IActionResult AssignTask(int taskId, string employeeName)
        {
            var task = tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null && string.IsNullOrEmpty(task.AssegnatoA)) // Se il task non è già assegnato
            {
                task.AssegnatoA = employeeName; // Assegna il task al dipendente
                TempData["SuccessMessage"] = $"Task {taskId} assegnato a {employeeName} con successo!";
                return RedirectToAction("Task"); // Redirect alla lista dei task con il messaggio di successo
            }
            TempData["ErrorMessage"] = "Il task è già assegnato o non esiste.";
            return RedirectToAction("Task");  // Redirect con messaggio di errore
        }


        // Azione per visualizzare i task assegnati a un dipendente
        [HttpGet]
        public IActionResult MyAssignedTasks(string employeeName)
        {
            // Filtra i task assegnati al dipendente specificato
            var assignedTasks = tasks.Where(t => t.AssegnatoA == employeeName).ToList();
            return View(assignedTasks); // Mostra i task assegnati al dipendente
        }

        // Azione per visualizzare tutti i task (sia per responsabili che dipendenti)
        [HttpGet]
        public IActionResult Task(string userRole)
        {
            var filteredTasks = tasks.Where(t =>
                (userRole == "Interno" && t.Tipologia == "Interno") ||
                (userRole == "Esterno" && t.Tipologia == "Esterno") ||
                (userRole == "Amministratore")).ToList(); // "Amministratore" può vedere entrambi
            return View(filteredTasks);
        }

        // Azione per creare un nuovo task
        [HttpPost]
        public IActionResult Create([FromBody] TaskViewModel newTask, string userRole)
        {
            if (userRole == "Esterno" && newTask.Tipologia != "Esterno")
            {
                TempData["ErrorMessage"] = "Il responsabile esterno può solo creare task di tipo Esterno.";
                return RedirectToAction("Task");  // Redirect alla pagina dei task con il messaggio di errore
            }

            newTask.Id = tasks.Count + 1;
            newTask.Completo = false;
            tasks.Add(newTask);

            TempData["SuccessMessage"] = "Task creato con successo!";
            return RedirectToAction("Task");  // Redirect alla lista dei task con il messaggio di successo
        }


        // Azione per marcare un task come completato
        [HttpPost]
        public IActionResult MarkComplete(int id, string userRole)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                TempData["ErrorMessage"] = "Task non trovato.";
                return RedirectToAction("Task");  // Ritorna alla lista dei task con un messaggio di errore
            }

            // Permetti di completare solo task che corrispondono al ruolo dell'utente
            // Solo i dipendenti possono completare qualsiasi tipo di task
            if (userRole != "Dipendente" && (userRole == "Responsabile Esterno" && task.Tipologia != "Esterno"))
            {
                TempData["ErrorMessage"] = "Un responsabile esterno può solo completare task esterni.";
                return RedirectToAction("Task");  // Ritorna alla lista dei task con un messaggio di errore
            }

            task.Completo = true;
            TempData["SuccessMessage"] = "Task completato con successo!";
            return RedirectToAction("Task");  // Redirect alla lista dei task con un messaggio di successo
        }



        // Azione per modificare un task
        [HttpPost]
        public IActionResult UpdateTask([FromBody] TaskViewModel updatedTask, string userRole)
        {
            var task = tasks.FirstOrDefault(t => t.Id == updatedTask.Id);
            if (task == null)
            {
                TempData["ErrorMessage"] = "Task non trovato.";
                return RedirectToAction("Task");  // Ritorna alla lista dei task con un messaggio di errore
            }

            // Logica di autorizzazione basata sul ruolo
            if (userRole == "Responsabile Esterno" && updatedTask.Tipologia != "Esterno")
            {
                TempData["ErrorMessage"] = "Un responsabile esterno non può cambiare la tipologia del task.";
                return RedirectToAction("Task");  // Ritorna alla lista dei task con un messaggio di errore
            }

            task.Descrizione = updatedTask.Descrizione;
            task.Tipologia = updatedTask.Tipologia;
            task.Priorità = updatedTask.Priorità;
            task.Scadenza = updatedTask.Scadenza;

            TempData["SuccessMessage"] = "Task aggiornato con successo!";
            return RedirectToAction("Task");  // Redirect alla lista dei task con un messaggio di successo
        }


        // Azione per eliminare un task
        [HttpPost]
        public IActionResult DeleteTask(int id, string userRole)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                TempData["ErrorMessage"] = "Task non trovato.";
                return RedirectToAction("Task");  // Ritorna alla lista dei task con un messaggio di errore
            }

            // Logica di autorizzazione basata sul ruolo
            if (userRole == "Responsabile Esterno" && task.Tipologia == "Interno")
            {
                TempData["ErrorMessage"] = "Un responsabile esterno non può eliminare task interni.";
                return RedirectToAction("Task");  // Ritorna alla lista dei task con un messaggio di errore
            }

            tasks.Remove(task);
            TempData["SuccessMessage"] = "Task eliminato con successo!";
            return RedirectToAction("Task");  // Redirect alla lista dei task con un messaggio di successo
        }

    }
}
