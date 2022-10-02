using GeekSpot.Domain.Entities;
using GeekSpot.Domain.Interfaces;
using GeekSpot.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeekSpot.UI.Controllers
{
    public class BlogController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogRepositoy _blogRepository;
        public BlogController(ILogger<HomeController> logger, IBlogRepositoy blogRepositoy)
        {
            _logger = logger;
            _blogRepository = blogRepositoy;
        }
        public async Task<IActionResult> Post(int id)
        {
            var post = await _blogRepository.GetByIdAsync(id);
            return View(new PostViewModel() { Post = post ?? new Post() });
        }
    }
}
