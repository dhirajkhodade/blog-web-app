using GeekSpot.Domain.Entities;

namespace GeekSpot.UI.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Post> PopularPosts { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
