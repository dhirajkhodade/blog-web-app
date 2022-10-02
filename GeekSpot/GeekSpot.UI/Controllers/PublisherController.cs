using GeekSpot.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekSpot.UI.Controllers
{
    public class PublisherController : Controller
    {
        const string SessionUserId = "_Id";
        const string SessionName = "_Name";

        public IActionResult Login()
        {
            return View();
        }

        //Stupid Auth code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                var userDetails = GetUserDetails();
                if (user.UserName.Equals(userDetails.UserName) && user.Password.Equals(userDetails.Password))
                {
                    HttpContext.Session.SetInt32(SessionUserId, userDetails.UserId);
                    HttpContext.Session.SetString(SessionName, "Kevin Mitnick");
                    return RedirectToAction("UserDashBoard");
                }
            }
            return View(user);
        }

        public ActionResult UserDashBoard()
        {
            var userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId != null)
            {
                ViewBag.Name = HttpContext.Session.GetString(SessionName);
                return View("Dashboard");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        private User GetUserDetails()
        {
            return new User(1234567890, "author", "password");
        }
    }
}
