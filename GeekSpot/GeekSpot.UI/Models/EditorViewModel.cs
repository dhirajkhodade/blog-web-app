using GeekSpot.Domain.Entities;

namespace GeekSpot.UI.Models
{
    public class EditorViewModel
    {
        public EditorViewModel()
        {
            Post = new Post();  
        }
        public Post Post { get; set; }
    }
}
