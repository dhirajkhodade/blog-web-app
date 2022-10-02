using GeekSpot.Domain.Interfaces;
using GeekSpot.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeekSpot.UI.Components
{
    public class RecentPostsViewComponent : ViewComponent
    {
        private readonly IBlogRepositoy _blogRepository;
        public RecentPostsViewComponent(IBlogRepositoy blogRepositoy)
        {
            _blogRepository = blogRepositoy;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var RecentPosts = await _blogRepository.GetAllAsync();
            var RecentPostsVm = new RecentPostsViewModel();
            RecentPostsVm.RecentPosts = RecentPosts;
            return View(RecentPostsVm);
        }
    }
}
