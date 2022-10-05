using GeekSpot.Domain.Interfaces;
using GeekSpot.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeekSpot.UI.Components
{
    public class RecentPostsViewComponent : ViewComponent
    {
        private readonly IBlogRepositoy _blogRepository;
        private readonly ILogger<RecentPostsViewComponent> _logger;
        public RecentPostsViewComponent(IBlogRepositoy blogRepositoy, ILogger<RecentPostsViewComponent> logger)
        {
            _blogRepository = blogRepositoy;
            _logger = logger;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var RecentPosts = await _blogRepository.GetAllAsync();
                var RecentPostsVm = new RecentPostsViewModel();
                RecentPostsVm.RecentPosts = RecentPosts.Take(3);
                return View(RecentPostsVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(new RecentPostsViewModel());
        }
    }
}
