using GeekSpot.Domain.Interfaces;
using GeekSpot.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekSpot.UI.Controllers
{
    public class PublisherController : Controller
    {
        const string SessionUserId = "_Id";
        const string SessionName = "_Name";
        private readonly ILogger<PublisherController> _logger;
        private readonly IBlogRepositoy _blogRepository;
        public PublisherController(IBlogRepositoy blogRepositoy, ILogger<PublisherController> logger)
        {
            _blogRepository = blogRepositoy;
            _logger = logger;
        }
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32(SessionUserId) != null)
            {
                return RedirectToAction("UserDashBoard");
            }
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

        public async Task<ActionResult> UserDashBoard()
        {
            var userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId != null)
            {
                ViewBag.Name = HttpContext.Session.GetString(SessionName);
                ViewData["_Id"] = HttpContext.Session.GetInt32(SessionUserId);
                var posts = await _blogRepository.GetAllAsync(true);
                return View("Dashboard",posts);
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
