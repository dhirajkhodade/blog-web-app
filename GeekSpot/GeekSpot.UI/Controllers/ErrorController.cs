using Microsoft.AspNetCore.Mvc;

namespace GeekSpot.UI.Controllers
{
    [Route("Error/{statuscode}")]
    public class ErrorController : Controller
    {
        public IActionResult Index(int statuscode)
        {
            switch (statuscode)
            {
                case 404:
                    ViewData["Error"] = "Page Not Found";
                    break;
                default:
                    ViewData["Error"] = "Something went wrong!";
                    break;
            }
            return View("Error");
        }
    }
}
