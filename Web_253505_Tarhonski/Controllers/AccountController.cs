using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Web_253505_Tarhonski.Models;
using Web_253505_Tarhonski.Sevices.AuthService;

namespace Web_253505_Tarhonski.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return View(new RegisterUserViewModel());
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel user, [FromServices] IAuthService authService)
        {
            if (ModelState.IsValid)
            {
                if (user == null)
                {
                    return BadRequest();
                }

                var result = await authService.RegisterUserAsync(user.Email, user.Password, user.Avatar);
                if (result.Result)
                {
                    return Redirect(Url.Action("Index", "Home"));
                }
                else
                {
                    return BadRequest(result.ErrorMessage);
                }
            }

            return View(user);
        }
        public async Task Login()
        {
            //Console.WriteLine(Url.Action("Index", "Home") + "888888888888888");
            await HttpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme,
                                             new AuthenticationProperties
                                             {
                                                 RedirectUri = "/"
                                             });
        }

        [HttpPost]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme,
                                           new AuthenticationProperties
                                           {
                                               RedirectUri = "/"
                                           });
        }

    }
}
