using GeekSpot.Domain.Entities;
using GeekSpot.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeekSpot.UI.Components
{
    public class TagCloudViewComponent : ViewComponent
    {
        private readonly IBlogRepositoy _blogRepository;
        private readonly ILogger<TagCloudViewComponent> _logger;
        public TagCloudViewComponent(IBlogRepositoy blogRepositoy, ILogger<TagCloudViewComponent> logger)
        {
            _blogRepository = blogRepositoy;
            _logger = logger;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                return View(await _blogRepository.GettagsAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(new List<Tag>());
        }
    }
}
