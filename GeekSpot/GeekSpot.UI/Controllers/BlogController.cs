using GeekSpot.Domain.Entities;
using GeekSpot.Domain.Interfaces;
using GeekSpot.UI.Models;
using GeekSpot.UI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NuGet.Packaging;

namespace GeekSpot.UI.Controllers
{
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IBlogRepositoy _blogRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IHubContext<NotificationHub> _notificationHub;
        public BlogController(ILogger<BlogController> logger, IBlogRepositoy blogRepositoy, IWebHostEnvironment env, IHubContext<NotificationHub> hubContext)
        {
            _logger = logger;
            _blogRepository = blogRepositoy;
            _env = env;
            _notificationHub = hubContext;
        }
        public async Task<IActionResult> PostDetails(int id)
        {
            try
            {
                var post = await _blogRepository.GetByIdAsync(id);
                await _notificationHub.Clients.All.SendAsync("PostViewed", new { postid = post?.Id, views = post?.ReadCount });
                return View(new PostViewModel() { Post = post ?? new Post() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Error");
            }
        }
        [Authorize]
        public async Task<IActionResult> UnpublishedPostDetails(int id)
        {
            try
            {
                var post = await _blogRepository.GetByIdAsync(id, true);
                return View("PostDetails", new PostViewModel() { Post = post ?? new Post() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Error");
            }
        }
        [Authorize]
        public async Task<IActionResult> EditPost(int id)
        {
            try
            {
                var post = await _blogRepository.GetByIdAsync(id, true);
                return View(new EditorViewModel() { Post = post ?? new Post() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Error");
            }
        }
        [Authorize]
        public IActionResult CreatePost()
        {
            try
            {
                return View(new EditorViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNewPost(Post post, string tags)
        {
            try
            {
                post.PublishedOn = post.Published ? DateTime.Now : null;
                post.CreatedOn = post.LastModifiedOn = DateTime.Now;
                post.ReadCount = 0;
                post.Tags.AddRange(tags.Split(',').ToList().Select(a => new Tag() { Name = a }));
                await _blogRepository.CreateAsync(post);
                await _notificationHub.Clients.All.SendAsync("PostPublish", "New post published. Check it out!");
                return RedirectToAction("UserDashBoard", "Publisher");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Error");
            }
        }
        [Authorize]
        public async Task<IActionResult> UpdatePost(Post post, string tags)
        {
            try
            {
                post.LastModifiedOn = DateTime.Now;
                PrepareTags(post, tags);
                await _blogRepository.UpdateAsync(post);
                return RedirectToAction("UserDashBoard", "Publisher");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Error");
            }
        }
        [Authorize]
        public async Task<IActionResult> PublishPost(int id)
        {
            try
            {
                var post = await _blogRepository.GetByIdAsync(id, true);
                post.Published = true;
                post.PublishedOn = DateTime.Now;
                post.LastModifiedOn = DateTime.Now;
                await _blogRepository.UpdateAsync(post);
                await _notificationHub.Clients.All.SendAsync("PostPublish", "New post published. Check it out!");
                return RedirectToAction("UserDashBoard", "Publisher");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Error");
            }
        }

        [Authorize]
        public async Task<IActionResult> UnPublishPost(int id)
        {
            try
            {
                var post = await _blogRepository.GetByIdAsync(id, true);
                post.Published = false;
                post.LastModifiedOn = DateTime.Now;
                await _blogRepository.UpdateAsync(post);
                await _notificationHub.Clients.All.SendAsync("PostUnPublish");
                return RedirectToAction("UserDashBoard", "Publisher");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Error");
            }
        }

        [Authorize]
        public IActionResult UploadImage(IFormFile file)
        {
            try
            {
                var location = new FileManager(_env).SaveImageToDisk(file);
                return Json(new { location });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Error");
            }
        }
        private void PrepareTags(Post post, string tags)
        {
            try
            {
                HashSet<string> newTags = new HashSet<string>(tags.Split(',').Select(tag => tag.ToLower()));
                post.Tags.RemoveAll(currentTag => !newTags.Contains(currentTag.Name.ToLower()));

                foreach (var tagName in tags.Split(','))
                {
                    if (!post.Tags.Any(t => t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase)))
                        post.Tags.Add(new Tag() { Name = tagName });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }


    }
}
