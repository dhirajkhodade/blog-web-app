using GeekSpot.Domain.Entities;

namespace GeekSpot.UI.Models
{
    public class RecentPostsViewModel
    {
        public IEnumerable<Post> RecentPosts { get; set; }
    }
}
