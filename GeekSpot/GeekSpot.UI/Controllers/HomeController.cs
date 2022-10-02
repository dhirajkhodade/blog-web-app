using GeekSpot.Domain.Entities;
using GeekSpot.Domain.Interfaces;
using GeekSpot.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GeekSpot.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogRepositoy _blogRepository;
        public HomeController(ILogger<HomeController> logger, IBlogRepositoy blogRepositoy)
        {
            _logger = logger;
            _blogRepository = blogRepositoy;
        }

        public async Task<IActionResult> Index()
        {
            var Posts = new IndexViewModel();
            Posts.Posts = await _blogRepository.GetAllAsync();
            Posts.PopularPosts = await _blogRepository.GetPopularPostsAsync(3);
            return View(Posts);
        }
        public IActionResult GetRecentPosts()
        {
            return ViewComponent("RecentPosts");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}