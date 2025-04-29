using Template.Web.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Template.Services.Shared;
using System.Threading.Tasks;
using Template.Infrastructure;

namespace Template.Web.Features.Login
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Alerts]
    [ModelStateToTempData]
    public partial class LoginController : Controller
    {
        public static string LoginErrorModelStateKey = "LoginError";
        private readonly SharedService _sharedService;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public LoginController(SharedService sharedService, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _sharedService = sharedService;
            _sharedLocalizer = sharedLocalizer;
        }

        private ActionResult LoginAndRedirect(UserDetailDTO utente, string returnUrl, bool rememberMe)
        {
            // Creazione dei claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, utente.Id.ToString()),
                new Claim(ClaimTypes.Email, utente.Email),
                new Claim(ClaimTypes.GivenName, utente.Nome),
                new Claim(ClaimTypes.Surname, utente.Cognome),
                new Claim(ClaimTypes.Name, utente.Username),
                new Claim(ClaimTypes.Role, utente.Ruolo)
            };

            // Debug: stampa dei claims in console per il controllo
            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim: {claim.Type} - {claim.Value}");
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
            {
                ExpiresUtc = (rememberMe) ? DateTimeOffset.UtcNow.AddMonths(3) : null,
                IsPersistent = rememberMe,
            });

            // Se c'è un returnUrl valido, redirigi, altrimenti alla AreaPersonale
            if (string.IsNullOrWhiteSpace(returnUrl) == false)
                return Redirect(returnUrl);

            return RedirectToAction("AreaPersonale", "AreaPersonale");
        }

        [HttpGet]
        public virtual IActionResult Login(string returnUrl)
        {
            if (HttpContext.User != null && HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                if (string.IsNullOrWhiteSpace(returnUrl) == false)
                    return Redirect(returnUrl);

                return RedirectToAction("AreaPersonale", "AreaPersonale");
            }

            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
            };

            return View(model);
        }

        [HttpPost]
        public async virtual Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verifica credenziali utente
                    var utente = await _sharedService.Query(new CheckLoginCredentialsQuery
                    {
                        Email = model.Email,
                        Password = model.Password,
                    });

                    // Se l'utente è valido esegue il login e reindirizza
                    return LoginAndRedirect(utente, model.ReturnUrl, model.RememberMe);
                }
                catch (LoginException e)
                {
                    // Se il login fallisce mostra errore
                    ModelState.AddModelError(LoginErrorModelStateKey, e.Message);
                }
            }

            return RedirectToAction(MVC.Login.Login());
        }

        [HttpPost]
        public virtual IActionResult Logout()
        {
            HttpContext.SignOutAsync();

            Alerts.AddSuccess(this, "Utente scollegato correttamente");
            return RedirectToAction(MVC.Login.Login());
        }
    }
}
