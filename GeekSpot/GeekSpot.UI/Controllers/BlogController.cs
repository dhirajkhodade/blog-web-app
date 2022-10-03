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
            return View(new EditorViewModel() { Post = post ?? new Post() });
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(string title, string content)
        {
            var post = await _blogRepository.GetByIdAsync(1);
            return View(new PostViewModel() { Post = post ?? new Post() });
        }
        public async Task<IActionResult> UpdatePost(Post post, string tags)
        {
            post.LastModifiedOn = DateTime.Now;
            PrepareTags(post, tags);
            await _blogRepository.UpdateAsync(post);
            return RedirectToAction("UserDashBoard", "Publisher");
        }
       
        public IActionResult UploadImage(IFormFile file)
        {
            var location = new FileManager(_env).SaveImageToDisk(file);
            return Json(new { location });

        }
        private static void PrepareTags(Post post, string tags)
        {
            HashSet<string> newTags = new HashSet<string>(tags.Split(',').Select(tag => tag.ToLower()));
            post.Tags.RemoveAll(currentTag => !newTags.Contains(currentTag.Name.ToLower()));

            foreach (var tagName in tags.Split(','))
            {
                if (!post.Tags.Any(t => t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase)))
                    post.Tags.Add(new Tag() { Name = tagName });
            }
        }


    }
}
