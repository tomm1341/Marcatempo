using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Template.Services.Shared;
using Template.Web.Infrastructure;
using Template.Web.SignalR;
using Template.Web.SignalR.Hubs.Events;
using Template.Web.Features.Task;
using Template.Web.Areas;

namespace Template.Web.Features.Task

{
    [Authorize(Roles = "ResponsabileInterno,ResponsabileEsterno")]
    public partial class TaskController : AuthenticatedBaseController
    {
        private readonly SharedService _sharedService;
        private readonly IPublishDomainEvents _publisher;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public TaskController(SharedService sharedService, IPublishDomainEvents publisher, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _sharedService = sharedService;
            _publisher = publisher;
            _sharedLocalizer = sharedLocalizer;

            ModelUnbinderHelpers.ModelUnbinders.Add(typeof(TaskViewModel), new SimplePropertyModelUnbinder());
        }

        [HttpGet]
        public virtual async Task<IActionResult> Task(TaskViewModel model)
        {

            return View(model);
        }


        [HttpGet]
        public virtual IActionResult New()
        {
            return RedirectToAction(Actions.Edit());
        }

        [HttpGet]
        public virtual async Task<IActionResult> Edit(Guid? id)
        {
            var model = new TaskViewModel();

            //if (id.HasValue)
            //{
            //    model.SetTask(await _sharedService.Query(new TaskDetailQuery
            //    {
            //        Id = id.Value,
            //    }));
            //}

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Edit(TaskViewModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        model.Id = await _sharedService.Handle(model.ToAddOrUpdateTaskCommand());

            //        Alerts.AddSuccess(this, "Task salvato correttamente");

            //        await _publisher.Publish(new NewMessageEvent
            //        {
            //            IdGroup = model.Id.Value,
            //            IdUser = CurrentUserId, // metodo dal base controller
            //            IdMessage = Guid.NewGuid()
            //        });
            //    }
            //    catch (Exception e)
            //    {
            //        ModelState.AddModelError(string.Empty, e.Message);
            //    }
            //}

            //if (!ModelState.IsValid)
            //{
            //    Alerts.AddError(this, "Errore nel salvataggio");
            //}

            return RedirectToAction(Actions.Edit(model.Id));
        }

        [HttpPost]
        public virtual async Task<IActionResult> Delete(Guid id)
        {
            //var task = await _sharedService.Query(new TaskDetailQuery { Id = id });

            //if (task.IdCreatore != CurrentUserId)
            //{
            //    return Unauthorized();
            //}

            //await _sharedService.Handle(new DeleteTaskCommand { Id = id });

            //Alerts.AddSuccess(this, "Task eliminato");

            return RedirectToAction(Actions.Task());
        }
    }
}
