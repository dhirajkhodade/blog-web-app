﻿using GeekSpot.Domain.Entities;
using GeekSpot.Domain.Interfaces;
using GeekSpot.UI.Models;
using GeekSpot.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Xml.Linq;

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
            var Posts = new IndexViewModel();
            Posts.Posts = await _blogRepository.GetAllAsync();
            Posts.PopularPosts = await _blogRepository.GetPopularPostsAsync(4);
            return View(Posts);
        }
        public async Task<IActionResult> GetPostsByTag(string name)
        {
            var Posts = new IndexViewModel();
            Posts.Posts = await _blogRepository.FindAsync(post => post.Tags.Any(t => t.Name == name));
            Posts.PopularPosts = await _blogRepository.GetPopularPostsAsync(4);
            return View("Index",Posts);
        }
        [HttpPost]
        public async Task<JsonResult> SearchPosts(string searchKeyword)
        {
            var post = await _blogRepository.FindAsync(p=>p.Title.ToLower().Contains(searchKeyword.ToLower()) && p.Published );
            var result = post.Select(p=> new { p.Title , p.Id });
            return Json(result);
        }

        public IActionResult GetRecentPosts()
        {
            return ViewComponent("RecentPosts");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}