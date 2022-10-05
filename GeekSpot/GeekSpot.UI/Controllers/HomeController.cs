using GeekSpot.Domain.Interfaces;
using GeekSpot.UI.Models;
using GeekSpot.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace GeekSpot.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogRepositoy _blogRepository;
        private readonly IHubContext<NotificationHub> _notificationHub;
        public HomeController(ILogger<HomeController> logger, IBlogRepositoy blogRepositoy, IHubContext<NotificationHub> hubContext)
        {
            _logger = logger;
            _blogRepository = blogRepositoy;
            _notificationHub = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var originalPosts = await _blogRepository.GetAllAsync();
                var posts = new IndexViewModel();

                if (originalPosts != null)
                {
                    posts.Posts = Helper.GetTruncatedTextFromHtml(originalPosts, 400);
                    posts.PopularPosts = await _blogRepository.GetPopularPostsAsync(4);
                }
                return View(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Error");
            }
            
        }
        public async Task<IActionResult> GetPostsByTag(string name)
        {
            try
            {
                var posts = new IndexViewModel();
                var originalPosts = await _blogRepository.FindAsync(post => post.Tags.Any(t => t.Name == name));
                posts.Posts = Helper.GetTruncatedTextFromHtml(originalPosts, 400);
                posts.PopularPosts = await _blogRepository.GetPopularPostsAsync(4);
                return View("Index", posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Error");
            }
          
        }
        [HttpPost]
        public async Task<JsonResult> SearchPosts(string searchKeyword)
        {
            try
            {
                var post = await _blogRepository.FindAsync(p => p.Title.ToLower().Contains(searchKeyword.ToLower()) && p.Published);
                var result = post.Select(p => new { p.Title, p.Id });
                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Json("");
        }

        public IActionResult GetRecentPosts()
        {
            try
            {
                return ViewComponent("RecentPosts");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}