using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekSpot.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<ImageUri> Images { get; set; }
        public List<Tag> Tags { get; set; }
        public int ReadCount { get; set; }
    }
}
