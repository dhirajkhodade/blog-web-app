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

        public int ReadCount { get; set; }

        public bool Published { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime PublishedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }

        //Navigation props
        public Author Author { get; set; }
        public List<ImageUri> Images { get; set; } = new List<ImageUri>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
