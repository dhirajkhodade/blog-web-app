using GeekSpot.Domain.Entities;
using GeekSpot.Domain.Interfaces;
using GeekSpot.UI.Models;
using GeekSpot.UI.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GeekSpot.UI.Controllers
{
    public class BlogController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogRepositoy _blogRepository;
        private readonly IWebHostEnvironment _env;
        public BlogController(ILogger<HomeController> logger, IBlogRepositoy blogRepositoy, IWebHostEnvironment env)
        {
            _logger = logger;
            _blogRepository = blogRepositoy;
            _env = env;
        }
        public async Task<IActionResult> PostDetails(int id)
        {
            var post = await _blogRepository.GetByIdAsync(id);
            return View(new PostViewModel() { Post = post ?? new Post() });
        }

        public async Task<IActionResult> EditPost(int id)
        {
            var post = await _blogRepository.GetByIdAsync(id);
            return View(new PostViewModel() { Post = post ?? new Post() });
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(string title, string content)
        {
            var post = await _blogRepository.GetByIdAsync(1);
            return View(new PostViewModel() { Post = post ?? new Post() });
        }

        public IActionResult UploadImage(IFormFile file)
        {
            var location = new FileManager(_env).SaveImageToDisk(file);
            return Json(new { location });

        }


    }
}
