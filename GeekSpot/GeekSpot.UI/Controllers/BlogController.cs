using GeekSpot.Domain.Entities;
using GeekSpot.Domain.Interfaces;
using GeekSpot.UI.Models;
using GeekSpot.UI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;

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
        [Authorize]
        public async Task<IActionResult> UnpublishedPostDetails(int id)
        {
            var post = await _blogRepository.GetByIdAsync(id,true);
            return View("PostDetails",new PostViewModel() { Post = post ?? new Post() });
        }
        [Authorize]
        public async Task<IActionResult> EditPost(int id)
        {
            var post = await _blogRepository.GetByIdAsync(id,true);
            return View(new EditorViewModel() { Post = post ?? new Post() });
        }
        [Authorize]
        public IActionResult CreatePost()
        {
            return View(new EditorViewModel());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNewPost(Post post, string tags)
        {
            post.PublishedOn = post.Published ? DateTime.Now : null;
            post.CreatedOn = post.LastModifiedOn = DateTime.Now;
            post.ReadCount = 0;
            post.Tags.AddRange(tags.Split(',').ToList().Select(a=> new Tag() { Name = a }));
            await _blogRepository.CreateAsync(post);
            return RedirectToAction("UserDashBoard", "Publisher");
        }
        [Authorize]
        public async Task<IActionResult> UpdatePost(Post post, string tags)
        {
            post.LastModifiedOn = DateTime.Now;
            PrepareTags(post, tags);
            await _blogRepository.UpdateAsync(post);
            return RedirectToAction("UserDashBoard", "Publisher");
        }
        [Authorize]
        public async Task<IActionResult> PublishPost(int id)
        {
            var post = await _blogRepository.GetByIdAsync(id, true);
            post.Published = true;
            post.PublishedOn = DateTime.Now;
            await _blogRepository.UpdateAsync(post);
            return RedirectToAction("UserDashBoard", "Publisher");
        }

        [Authorize]
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
