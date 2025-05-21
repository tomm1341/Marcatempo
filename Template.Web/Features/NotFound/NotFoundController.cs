using Microsoft.AspNetCore.Mvc;

namespace Template.Web.Features.NotFound
{
    public partial class NotFoundController : Controller
    {
        [Route("NotFound")]
        public virtual IActionResult Index(int statusCode = 404)
        {
            // Imposta 404 anche nella response (già automatico, ma per sicurezza)
            Response.StatusCode = statusCode;
            return View("NotFound");
        }
    }
}
