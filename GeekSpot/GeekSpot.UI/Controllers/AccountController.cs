using GeekSpot.UI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GeekSpot.Domain.Interfaces;

namespace GeekSpot.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly string _password = "password";
        public AccountController(IUserRepository userRepository, ILogger<AccountController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }
        public IActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel loginDetails)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.FindAsync(u => u.Name.ToLower() == loginDetails.UserName.ToLower() && _password == loginDetails.Password);
                if (user == null)
                {
                    ViewBag.Message = "Invalid Credential";
                    return View(loginDetails);
                }
                else
                {
                    //create claims
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)),
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Surname, user.Surname)
                        };
                    //create claims identity
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //create claims principal    
                    var principal = new ClaimsPrincipal(identity);
                    //SignIn
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                    {
                        IsPersistent = loginDetails.RememberLogin
                    });
                    HttpContext.Session.SetString("username",loginDetails.UserName);
                    return RedirectToAction("UserDashBoard", "Publisher");
                }
            }
            return View(loginDetails);
        }

        public async Task<IActionResult> LogOut()
        {
            //SignOut
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            //Redirect to landing page
            return RedirectToAction("Index", "Home");
        }
    }
}
