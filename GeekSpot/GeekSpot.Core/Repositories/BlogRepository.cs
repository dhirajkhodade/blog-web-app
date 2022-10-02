using GeekSpot.Domain.Entities;
using GeekSpot.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GeekSpot.Core.Repositories
{
    public class BlogRepository :IBlogRepositoy
    {
        public readonly AppDbContext _dbContext;

        public BlogRepository(AppDbContext context)
        {
            _dbContext = context;
        }
        public async Task CreateAsync(Post entity)
        {
            _dbContext.Posts.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Post entity)
        {
            _dbContext.Posts.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Post>> FindAsync(Expression<Func<Post, bool>> expression)
        {
            return await _dbContext.Posts
                .Include(post => post.Tags)
                .Include(post => post.Images)
                .Include(post => post.Author)
                .Where(expression)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _dbContext.Posts
                .Include(post=>post.Tags)
                .Include(post=>post.Images)
                .Include(post=>post.Author)
                .ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            return await _dbContext.Posts
                .Where(post => post.Id == id)
                .Include(post => post.Tags)
                .Include(post => post.Images)
                .Include(post => post.Author)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Post entity)
        {
            _dbContext.Posts.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Post>> GetPopularPostsAsync(int count)
        {
            return await _dbContext.Posts.OrderByDescending(p => p.ReadCount).Take(count).ToListAsync();
        }

        public async Task<IEnumerable<Tag>> GettagsAsync()
        {
            return await _dbContext.tags.Distinct().ToListAsync();
        }
    }
}
