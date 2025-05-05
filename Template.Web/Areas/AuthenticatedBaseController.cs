using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;
using Template.Web.Infrastructure;

namespace Template.Web.Areas
{
    [Authorize]
    [Alerts]
    [ModelStateToTempData]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public partial class AuthenticatedBaseController : Controller
    {
        public AuthenticatedBaseController() { }

        //protected Guid CurrentUserId => Identita?.IdUtenteCorrente ?? Guid.Empty;


        protected IdentitaViewModel Identita
        {
            get
            {
                return (IdentitaViewModel)ViewData[IdentitaViewModel.VIEWDATA_IDENTITACORRENTE_KEY];
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext?.User?.Identity?.IsAuthenticated == true)
            {
                var userClaims = context.HttpContext.User.Claims;

                var idClaim = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                Guid.TryParse(idClaim, out var idUtente);

                ViewData[IdentitaViewModel.VIEWDATA_IDENTITACORRENTE_KEY] = new IdentitaViewModel
                {
                    IdUtenteCorrente = idUtente,
                    EmailUtenteCorrente = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    NomeUtenteCorrente = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value,
                    CognomeUtenteCorrente = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value,
                    UsernameUtenteCorrente = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                    RuoloUtenteCorrente = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
                };
            }
            else
            {
                // Logout automatico se l'utente non è autenticato
                HttpContext.SignOutAsync();
                SignOut();

                context.Result = new RedirectResult(context.HttpContext.Request.GetEncodedUrl());
                Alerts.AddError(this, "L'utente non possiede i diritti per visualizzare la risorsa richiesta");
            }

            base.OnActionExecuting(context);
        }
    }
}
