using GeekSpot.Domain.Interfaces;
using GeekSpot.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeekSpot.UI.Components
{
    public class TagCloudViewComponent : ViewComponent
    {
        private readonly IBlogRepositoy _blogRepository;
        public TagCloudViewComponent(IBlogRepositoy blogRepositoy)
        {
            _blogRepository = blogRepositoy;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _blogRepository.GettagsAsync());
        }
    }
}
