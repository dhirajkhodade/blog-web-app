using GeekSpot.Domain.Entities;
using GeekSpot.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekSpot.Core.Repositories
{
    public class BlogRepository : GenericRepository<Post>, IBlogRepositoy
    {   
        public BlogRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Post>> GetPopularPosts(int count)
        {
            return await _dbContext.Posts.OrderByDescending(p => p.ReadCount).Take(count).ToListAsync();
        }
    }
}
