using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Template.Services.Shared;
using Template.Web.Infrastructure;
using Template.Web.Areas;

namespace Template.Web.Features.Task
{
    [Authorize(Roles = "ResponsabileInterno,ResponsabileEsterno")]
    public partial class TaskController : AuthenticatedBaseController
    {
        private readonly SharedService _sharedService;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public TaskController(SharedService sharedService, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _sharedService = sharedService;
            _sharedLocalizer = sharedLocalizer;

            ModelUnbinderHelpers.ModelUnbinders.Add(typeof(CreateTaskCommand), new SimplePropertyModelUnbinder());
        }

        [HttpGet]
        public virtual IActionResult Task()
        {
            return View(new CreateTaskCommand
            {
                Stato = "InAttesa"
            });
        }


        [HttpPost]
        public virtual async Task<IActionResult> Create(CreateTaskCommand command)
        {
            if (!ModelState.IsValid)
                return View("Task", command);

            try
            {
                command.Id = Guid.NewGuid();
                command.Stato = "InAttesa"; 

                await _sharedService.Handle(command, CurrentUserId);

                return RedirectToAction("Disponibili", "Disponibili");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Task", command);
            }
        }
    }
}
