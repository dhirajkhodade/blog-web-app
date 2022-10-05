using GeekSpot.Domain.Interfaces;
using GeekSpot.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GeekSpot.UI.Controllers
{
    public class PublisherController : Controller
    {
        private readonly ILogger<PublisherController> _logger;
        private readonly IBlogRepositoy _blogRepository;
        public PublisherController(IBlogRepositoy blogRepositoy, ILogger<PublisherController> logger)
        {
            _blogRepository = blogRepositoy;
            _logger = logger;
        }
        [Authorize]
        public async Task<ActionResult> UserDashBoard()
        {
            try
            {
                var posts = await _blogRepository.GetAllAsync(true);
                return View("Dashboard", posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error","Error");
            }
          
        }
    }
}
