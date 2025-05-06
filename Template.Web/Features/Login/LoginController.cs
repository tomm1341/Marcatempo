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

        private async Task<ActionResult> LoginAndRedirect(UserDetailDTO utente, string returnUrl, bool rememberMe)
        {
            // Creating claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, utente.Id.ToString()),
                new Claim(ClaimTypes.Email, utente.Email),
                new Claim(ClaimTypes.GivenName, utente.Nome),
                new Claim(ClaimTypes.Surname, utente.Cognome),
                new Claim(ClaimTypes.Name, utente.Username),
                new Claim(ClaimTypes.Role, utente.Ruolo)
            };

            // (facoltativo) Debug dei claim
            // _logger.LogInformation("Claims user: {@claims}", claims);

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    ExpiresUtc = rememberMe
                        ? DateTimeOffset.UtcNow.AddMonths(3)
                        : (DateTimeOffset?)null,
                    IsPersistent = rememberMe,
                });

            // Redirect
            if (!string.IsNullOrWhiteSpace(returnUrl))
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
                    return await LoginAndRedirect(utente, model.ReturnUrl, model.RememberMe);
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
